namespace DineFearless.Models;

public enum AllergenType
{
    Gluten,
    Dairy,
    Peanuts,
    TreeNuts,
    Shellfish,
    FishSauceOrPaste,
    Eggs,
    Soy,
    Sesame,
    PorkOrLard   
}

public enum SensitivityLevel
{
    StrictMedical,
    MildSensitivity
}

public enum AssessmentStatus
{
    Safe,
    Modifiable,
    Unsafe
}