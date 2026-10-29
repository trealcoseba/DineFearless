namespace DineFearless.Services;

public class FavoritesService
{
    private readonly HashSet<string> _favoriteRestaurantNames = new() { "Artisan Kitchen" };

    public event Action? OnFavoritesChanged;

    public bool IsFavorite(string restaurantName) => _favoriteRestaurantNames.Contains(restaurantName);

    public void ToggleFavorite(string restaurantName)
    {
        if (!_favoriteRestaurantNames.Add(restaurantName))
            _favoriteRestaurantNames.Remove(restaurantName);

        OnFavoritesChanged?.Invoke();
    }
}