using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DineFearless.Models.Db;

[Table("dishes")]
public class DishRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("restaurant_id")]
    public long RestaurantId { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("price")]
    public decimal Price { get; set; }

    [Column("category")]
    public string Category { get; set; } = "Mains";

    [Column("image_url")]
    public string ImageUrl { get; set; } = "";

    [Column("uses_dedicated_fryer")]
    public bool UsesDedicatedFryer { get; set; }

    [Column("has_dedicated_prep_table")]
    public bool HasDedicatedPrepTable { get; set; }

    [Column("confirmed_notes")]
    public string ConfirmedNotes { get; set; } = "";

    [Column("ingredients_spec")]
    public string IngredientsSpec { get; set; } = "";

    [Column("modifications_csv")]
    public string ModificationsCsv { get; set; } = "";

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}