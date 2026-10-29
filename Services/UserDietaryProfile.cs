using DineFearless.Models;

namespace DineFearless.Services;

public class UserDietaryProfile
{
    private readonly HashSet<AllergenType> _activeAllergens = new() { AllergenType.Gluten, AllergenType.Dairy };
    
    public bool DedicatedFryerOnly { get; set; } = true;
    public bool SeparatePrepTable { get; set; } = true;
    public SensitivityLevel Sensitivity { get; private set; } = SensitivityLevel.StrictMedical;

    public event Action? OnProfileChanged;

    public IReadOnlySet<AllergenType> ActiveAllergens => _activeAllergens;

    public void ToggleAllergen(AllergenType allergen)
    {
        if (!_activeAllergens.Add(allergen))
            _activeAllergens.Remove(allergen);

        OnProfileChanged?.Invoke();
    }

    public void SetSensitivity(SensitivityLevel level)
    {
        Sensitivity = level;
        OnProfileChanged?.Invoke();
    }

    public void ToggleFryer() { DedicatedFryerOnly = !DedicatedFryerOnly; OnProfileChanged?.Invoke(); }
    public void TogglePrep() { SeparatePrepTable = !SeparatePrepTable; OnProfileChanged?.Invoke(); }

    public ISafetyEvaluationStrategy GetCurrentStrategy() => Sensitivity switch
    {
        SensitivityLevel.StrictMedical => new StrictMedicalStrategy(),
        SensitivityLevel.MildSensitivity => new MildSensitivityStrategy(),
        _ => new StrictMedicalStrategy()
    };

    public SafetyResult EvaluateItem(MenuItem item) =>
        GetCurrentStrategy().Evaluate(item, _activeAllergens, DedicatedFryerOnly, SeparatePrepTable);
}