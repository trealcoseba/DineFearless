using DineFearless.Models;

namespace DineFearless.Services;

public class RestaurantDataService
{
    private readonly List<Restaurant> _restaurants = new();

    public RestaurantDataService()
    {
        SeedData();
    }

    public IReadOnlyList<Restaurant> GetAll() => _restaurants.AsReadOnly();
    
    public Restaurant GetBySlug(string slug) => 
        _restaurants.FirstOrDefault(r => r.Name.Replace(" ", "-").ToLower() == slug.ToLower()) 
        ?? _restaurants.First();

    private void SeedData()
    {
        // 1. Artisan Kitchen
        var artisan = new Restaurant
        {
            Name = "Artisan Kitchen",
            Category = "Seasonal American",
            Neighborhood = "Williamsburg",
            Distance = "0.8 mi",
            Rating = 4.9,
            ReviewCount = 128,
            Hours = "Open until 10 pm",
            BannerImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?auto=format&fit=crop&w=1600&q=80"
        };

        // Dish 1: Safe
        var polenta = new FoodDish(
            "Truffle Polenta", 18.00m, "Golden corn polenta, wild mushrooms & truffle olive oil.",
            "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=600&q=80",
            usesDedicatedFryer: true, hasDedicatedPrepTable: true,
            confirmedNotes: "No gluten or dairy ingredients. Separate prep table confirmed."
        );
        polenta.AddIngredient(new Ingredient("Cornmeal"));
        polenta.AddIngredient(new Ingredient("Mushrooms"));
        polenta.AddIngredient(new Ingredient("Olive Oil"));
        artisan.Menu.Add(polenta);

        // Dish 2: Modifiable
        var risotto = new FoodDish(
            "Mushroom Risotto", 22.00m, "Arborio rice, forest mushrooms & house vegetable stock.",
            "https://images.unsplash.com/photo-1633964913295-ceb43826e7c9?auto=format&fit=crop&w=600&q=80",
            usesDedicatedFryer: true, hasDedicatedPrepTable: true
        );
        risotto.AddIngredient(new Ingredient("Arborio Rice"));
        risotto.AddIngredient(new Ingredient("Butter", AllergenType.Dairy));
        risotto.AddIngredient(new Ingredient("Parmesan", AllergenType.Dairy));
        risotto.AddModification("Omit butter");
        risotto.AddModification("Omit parmesan");
        artisan.Menu.Add(risotto);

        // Dish 3: Unsafe
        var pasta = new FoodDish(
            "Seafood Linguine", 26.00m, "Fresh linguine, shrimp & mussels in a cream sauce.",
            "https://images.unsplash.com/photo-1563379091339-03b21ab4a4f8?auto=format&fit=crop&w=600&q=80",
            usesDedicatedFryer: false, hasDedicatedPrepTable: false
        );
        pasta.AddIngredient(new Ingredient("Fresh Linguine", AllergenType.Gluten));
        pasta.AddIngredient(new Ingredient("Shrimp", AllergenType.Shellfish));
        pasta.AddIngredient(new Ingredient("Heavy Cream", AllergenType.Dairy));
        artisan.Menu.Add(pasta);

        _restaurants.Add(artisan);

        // 2. Green Fork Bistro
        var greenFork = new Restaurant
        {
            Name = "Green Fork Bistro",
            Category = "Farm-to-Table & Vegan",
            Neighborhood = "Greenpoint",
            Distance = "1.2 mi",
            Rating = 4.8,
            ReviewCount = 95,
            Hours = "Open until 9 pm",
            BannerImageUrl = "https://images.unsplash.com/photo-1555396273-367ea4eb4db5?auto=format&fit=crop&w=1600&q=80"
        };
        var harvestBowl = new FoodDish(
            "Quinoa Harvest Bowl", 16.00m, "Roasted sweet potatoes, avocado, quinoa, and tahini drizzle.",
            "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=600&q=80",
            usesDedicatedFryer: true, hasDedicatedPrepTable: true,
            confirmedNotes: "100% naturally gluten-free and dairy-free."
        );
        harvestBowl.AddIngredient(new Ingredient("Quinoa"));
        harvestBowl.AddIngredient(new Ingredient("Avocado"));
        greenFork.Menu.Add(harvestBowl);
        _restaurants.Add(greenFork);
    }
}