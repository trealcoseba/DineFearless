using DineFearless.Models.Db;
using static Supabase.Postgrest.Constants;

namespace DineFearless.Services;

public class AuditDataService
{
    private readonly Supabase.Client _db;

    public AuditDataService(Supabase.Client db)
    {
        _db = db;
    }

    // READ (Habit 2: Filter deleted_at == null in DB before fetching)
    public async Task<List<KitchenAuditRecord>> GetAuditsAsync()
    {
        var res = await _db.From<KitchenAuditRecord>()
            .Where(a => a.DeletedAt == null)
            .Order(a => a.Id, Ordering.Descending)
            .Range(0, 49)
            .Get();

        return res.Models;
    }

    // CREATE (POST)
    public async Task AddAuditAsync(long restaurantId, string restaurantName, string comment, string tagsCsv)
    {
        var record = new KitchenAuditRecord
        {
            RestaurantId = restaurantId,
            RestaurantName = restaurantName,
            Comment = comment,
            TagsCsv = tagsCsv,
            Upvotes = 1
        };

        await _db.From<KitchenAuditRecord>().Insert(record);
    }

    // UPDATE (PATCH upvotes)
    public async Task UpvoteAsync(long auditId, int newCount)
    {
        await _db.From<KitchenAuditRecord>()
            .Where(a => a.Id == auditId)
            .Set(a => a.Upvotes, newCount)
            .Update();
    }

    // SOFT DELETE (PATCH deleted_at)
    public async Task SoftDeleteAsync(long auditId)
    {
        await _db.From<KitchenAuditRecord>()
            .Where(a => a.Id == auditId)
            .Set(a => a.DeletedAt!, DateTime.UtcNow)
            .Update();
    }
}