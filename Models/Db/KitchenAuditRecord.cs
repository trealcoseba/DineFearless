using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DineFearless.Models.Db;

[Table("kitchen_audits")]
public class KitchenAuditRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("restaurant_id")]
    public long RestaurantId { get; set; }

    [Column("restaurant_name")]
    public string RestaurantName { get; set; } = "";

    [Column("comment")]
    public string Comment { get; set; } = "";

    [Column("tags_csv")]
    public string TagsCsv { get; set; } = "";

    [Column("upvotes")]
    public int Upvotes { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}