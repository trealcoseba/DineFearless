using DineFearless.Models;

namespace DineFearless.Services;

public record PlateItem(MenuItem Dish, int Quantity, string StatusNote);

public class SafePlateService
{
    private readonly List<PlateItem> _items = new();
    public event Action? OnPlateChanged;

    public IReadOnlyList<PlateItem> Items => _items.AsReadOnly();
    public decimal EstimatedTotal => _items.Sum(i => i.Dish.Price * i.Quantity);
    public int TotalCount => _items.Sum(i => i.Quantity);

    public void Add(MenuItem dish, string note = "Safe • no changes needed")
    {
        var existing = _items.FirstOrDefault(i => i.Dish.Id == dish.Id);
        if (existing is not null)
            _items[_items.IndexOf(existing)] = existing with { Quantity = existing.Quantity + 1 };
        else
            _items.Add(new PlateItem(dish, 1, note));

        OnPlateChanged?.Invoke();
    }

    public void Remove(MenuItem dish)
    {
        _items.RemoveAll(i => i.Dish.Id == dish.Id);
        OnPlateChanged?.Invoke();
    }

    public void UpdateQuantity(MenuItem dish, int delta)
    {
        var existing = _items.FirstOrDefault(i => i.Dish.Id == dish.Id);
        if (existing is null) return;

        var newQty = existing.Quantity + delta;
        if (newQty <= 0)
            _items.Remove(existing);
        else
            _items[_items.IndexOf(existing)] = existing with { Quantity = newQty };

        OnPlateChanged?.Invoke();
    }

    public bool Contains(MenuItem dish) => _items.Any(i => i.Dish.Id == dish.Id);
}