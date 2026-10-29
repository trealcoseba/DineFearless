namespace DineFearless.Models;

public abstract class MenuItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public decimal Price { get; }
    public string Description { get; }
    public string ImageUrl { get; }

    protected readonly List<Ingredient> Ingredients = new();
    protected readonly List<string> Modifications = new();

    protected MenuItem(string name, decimal price, string description, string imageUrl)
    {
        Name = name;
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void AddIngredient(Ingredient ingredient) => Ingredients.Add(ingredient);
    public void AddModification(string instruction) => Modifications.Add(instruction);

    public IReadOnlyList<Ingredient> GetIngredients() => Ingredients.AsReadOnly();
    public IReadOnlyList<string> GetModifications() => Modifications.AsReadOnly();

    public abstract bool HasCrossContaminationRisk(AllergenType allergen);
}

public class FoodDish : MenuItem
{
    public bool UsesDedicatedFryer { get; }
    public bool HasDedicatedPrepTable { get; }
    public string ConfirmedNotes { get; }

    public FoodDish(
        string name, 
        decimal price, 
        string description, 
        string imageUrl,
        bool usesDedicatedFryer, 
        bool hasDedicatedPrepTable, 
        string confirmedNotes = "")
        : base(name, price, description, imageUrl)
    {
        UsesDedicatedFryer = usesDedicatedFryer;
        HasDedicatedPrepTable = hasDedicatedPrepTable;
        ConfirmedNotes = confirmedNotes;
    }

    public override bool HasCrossContaminationRisk(AllergenType allergen)
    {
        if (allergen == AllergenType.Gluten && !UsesDedicatedFryer) return true;
        if (!HasDedicatedPrepTable) return true;
        return false;
    }
}