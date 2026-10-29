using DineFearless.Models;

namespace DineFearless.Services;

public record SafetyResult(
    AssessmentStatus Status, 
    string BadgeText, 
    string? ModificationNote = null, 
    string? UnsafeReason = null
);

public interface ISafetyEvaluationStrategy
{
    SafetyResult Evaluate(
        MenuItem item, 
        IReadOnlySet<AllergenType> activeAllergens, 
        bool requireDedicatedFryer, 
        bool requireDedicatedPrep
    );
}