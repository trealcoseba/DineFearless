using DineFearless.Models;

namespace DineFearless.Services;

public class StrictMedicalStrategy : ISafetyEvaluationStrategy
{
    public SafetyResult Evaluate(
        MenuItem item, 
        IReadOnlySet<AllergenType> activeAllergens, 
        bool requireDedicatedFryer, 
        bool requireDedicatedPrep)
    {
        // 1. Direct Allergen Match
        var directMatches = activeAllergens
            .Where(a => item.GetIngredients().Any(i => i.ContainsAllergen(a)))
            .ToList();

        if (directMatches.Any())
        {
            if (item.GetModifications().Any())
            {
                return new SafetyResult(
                    AssessmentStatus.Modifiable,
                    "Modifiable",
                    ModificationNote: string.Join("; ", item.GetModifications())
                );
            }

            return new SafetyResult(
                AssessmentStatus.Unsafe,
                $"Contains {string.Join(" & ", directMatches)}",
                UnsafeReason: "Also contains allergens matching your profile. No safe substitution available."
            );
        }

        // 2. Cross Contamination Protocols Check
        if (item is FoodDish food)
        {
            if (requireDedicatedFryer && !food.UsesDedicatedFryer)
                return new SafetyResult(AssessmentStatus.Unsafe, "Shared Fryer Risk", UnsafeReason: "Cooked in shared oil surface.");

            if (requireDedicatedPrep && !food.HasDedicatedPrepTable)
                return new SafetyResult(AssessmentStatus.Unsafe, "Shared Prep Risk", UnsafeReason: "No separate prep station verified.");
        }

        return new SafetyResult(AssessmentStatus.Safe, "Safe");
    }
}

public class MildSensitivityStrategy : ISafetyEvaluationStrategy
{
    public SafetyResult Evaluate(
        MenuItem item, 
        IReadOnlySet<AllergenType> activeAllergens, 
        bool requireDedicatedFryer, 
        bool requireDedicatedPrep)
    {
        var directMatches = activeAllergens
            .Where(a => item.GetIngredients().Any(i => i.ContainsAllergen(a)))
            .ToList();

        if (directMatches.Any())
        {
            if (item.GetModifications().Any())
                return new SafetyResult(AssessmentStatus.Modifiable, "Modifiable", string.Join("; ", item.GetModifications()));

            return new SafetyResult(AssessmentStatus.Unsafe, $"Contains {string.Join(" & ", directMatches)}", "Contains direct allergen.");
        }

        return new SafetyResult(AssessmentStatus.Safe, "Safe");
    }
}