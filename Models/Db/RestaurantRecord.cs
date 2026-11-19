using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DineFearless.Models.Db;

[Table("restaurants")]
public class RestaurantRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("slug")]
    public string Slug { get; set; } = "";

    [Column("category")]
    public string Category { get; set; } = "";

    [Column("neighborhood")]
    public string Neighborhood { get; set; } = "";

    [Column("distance")]
    public string Distance { get; set; } = "";

    [Column("rating")]
    public double Rating { get; set; }

    [Column("review_count")]
    public int ReviewCount { get; set; }

    [Column("hours")]
    public string Hours { get; set; } = "";

    [Column("banner_image_url")]
    public string BannerImageUrl { get; set; } = "";
}