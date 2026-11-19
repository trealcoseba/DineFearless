using DineFearless.Models;
using DineFearless.Models.Db;
using static Supabase.Postgrest.Constants;

namespace DineFearless.Services;

public class RestaurantDataService
{
    private readonly Supabase.Client _db;

    public RestaurantDataService(Supabase.Client db)
    {
        _db = db;
    }

    // READ ALL (Habit 2 + Habit 3: Fetches outside any loop -> 2 requests total, zero N+1!)
    public async Task<List<Restaurant>> GetAllAsync(string? searchFilter = null)
    {
        var restResponse = string.IsNullOrWhiteSpace(searchFilter)
            ? await _db.From<RestaurantRecord>()
                .Order(r => r.Id, Ordering.Ascending)
                .Range(0, 49)
                .Get()
            : await _db.From<RestaurantRecord>()
                .Filter("name", Operator.ILike, $"%{searchFilter}%")
                .Order(r => r.Id, Ordering.Ascending)
                .Range(0, 49)
                .Get();

        // Single bulk query for all non-deleted dishes (Never fetch inside a loop!)
        var dishResponse = await _db.From<DishRecord>()
            .Where(d => d.DeletedAt == null)
            .Order(d => d.Id, Ordering.Ascending)
            .Range(0, 199)
            .Get();

        var dishesByRestaurant = dishResponse.Models
            .GroupBy(d => d.RestaurantId)
            .ToDictionary(g => g.Key, g => g.ToList());

        return restResponse.Models.Select(r =>
        {
            var restaurant = MapToDomain(r);
            if (dishesByRestaurant.TryGetValue(r.Id, out var records))
            {
                restaurant.Menu = records.Select(MapDishToDomain).ToList();
            }
            return restaurant;
        }).ToList();
    }

    // READ ONE BY SLUG (Filtered in DB)
    public async Task<Restaurant?> GetBySlugAsync(string slug)
    {
        var restRes = await _db.From<RestaurantRecord>()
            .Where(r => r.Slug == slug)
            .Get();

        var record = restRes.Models.FirstOrDefault();
        if (record is null) return null;

        var dishRes = await _db.From<DishRecord>()
            .Where(d => d.RestaurantId == record.Id)
            .Where(d => d.DeletedAt == null) // Habit 2: Skip soft-deleted rows in DB
            .Order(d => d.Id, Ordering.Ascending)
            .Range(0, 49)
            .Get();

        var restaurant = MapToDomain(record);
        restaurant.Menu = dishRes.Models.Select(MapDishToDomain).ToList();
        return restaurant;
    }

    // CREATE DISH (POST)
    public async Task AddDishAsync(
        long restaurantId,
        string name,
        string description,
        decimal price,
        string category,
        string imageUrl,
        bool dedicatedFryer,
        bool dedicatedPrep,
        string modificationsCsv)
    {
        var newDish = new DishRecord
        {
            RestaurantId = restaurantId,
            Name = name,
            Description = description,
            Price = price,
            Category = category,
            ImageUrl = string.IsNullOrWhiteSpace(imageUrl)
                ? "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=600&q=80"
                : imageUrl,
            UsesDedicatedFryer = dedicatedFryer,
            HasDedicatedPrepTable = dedicatedPrep,
            ConfirmedNotes = dedicatedPrep ? "Verified allergen protocols." : "",
            IngredientsSpec = $"{name}|None",
            ModificationsCsv = modificationsCsv
        };

        await _db.From<DishRecord>().Insert(newDish);
    }

    // UPDATE DISH PRICE (PATCH)
    public async Task UpdateDishPriceAsync(long dishId, decimal newPrice)
    {
        if (newPrice < 0) return;

        await _db.From<DishRecord>()
            .Where(d => d.Id == dishId)
            .Set(d => d.Price, newPrice)
            .Update();
    }

    // SOFT DELETE DISH (PATCH deleted_at)
    public async Task SoftDeleteDishAsync(long dishId)
    {
        await _db.From<DishRecord>()
            .Where(d => d.Id == dishId)
            .Set(d => d.DeletedAt!, DateTime.UtcNow)
            .Update();
    }

    // Mapping helpers to keep OOP Strategy Pattern intact
    private static Restaurant MapToDomain(RestaurantRecord r) => new()
    {
        DbId = r.Id,
        Name = r.Name,
        Slug = r.Slug,
        Category = r.Category,
        Neighborhood = r.Neighborhood,
        Distance = r.Distance,
        Rating = r.Rating,
        ReviewCount = r.ReviewCount,
        Hours = r.Hours,
        BannerImageUrl = r.BannerImageUrl
    };

    private static MenuItem MapDishToDomain(DishRecord d)
    {
        var dish = new FoodDish(
            d.Name, d.Price, d.Description, d.ImageUrl,
            d.UsesDedicatedFryer, d.HasDedicatedPrepTable, d.ConfirmedNotes
        )
        {
            DbId = d.Id,
            Category = d.Category
        };

        // Parse IngredientsSpec: "Butter|Dairy;Arborio Rice|None"
        foreach (var entry in d.IngredientsSpec.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = entry.Split('|');
            var ingName = parts[0].Trim();
            if (parts.Length > 1 && Enum.TryParse<AllergenType>(parts[1].Trim(), out var allergen))
                dish.AddIngredient(new Ingredient(ingName, allergen));
            else
                dish.AddIngredient(new Ingredient(ingName));
        }

        // Parse ModificationsCsv: "Omit butter;Omit parmesan"
        foreach (var mod in d.ModificationsCsv.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            dish.AddModification(mod.Trim());
        }

        return dish;
    }
}