namespace DineFearless.Models;

public class Ingredient
{
    public string Name { get; }
    public bool IsCertifiedSupplier { get; }
    public string OriginNotes { get; }
    private readonly HashSet<AllergenType> _allergens = new();

    // Constructor 1: Used for simple dishes (Name + Allergens)
    public Ingredient(string name, params AllergenType[] allergens)
    {
        Name = name;
        IsCertifiedSupplier = true;
        OriginNotes = "Verified kitchen supplier";
        foreach (var a in allergens)
        {
            _allergens.Add(a);
        }
    }

    // Constructor 2: Used when origin/supplier details are specified
    public Ingredient(string name, bool isCertifiedSupplier, string originNotes, params AllergenType[] allergens)
    {
        Name = name;
        IsCertifiedSupplier = isCertifiedSupplier;
        OriginNotes = originNotes;
        foreach (var a in allergens)
        {
            _allergens.Add(a);
        }
    }

    public bool ContainsAllergen(AllergenType allergen) => _allergens.Contains(allergen);
    public IReadOnlySet<AllergenType> GetContainedAllergens() => _allergens;
}