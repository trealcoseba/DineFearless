namespace DineFearless.Models;

public class Restaurant
{
    public long DbId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public string Hours { get; set; } = string.Empty;
    public string BannerImageUrl { get; set; } = string.Empty;
    public List<MenuItem> Menu { get; set; } = new();
}