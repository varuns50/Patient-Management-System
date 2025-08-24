using System.ComponentModel.DataAnnotations;

public class NotInFutureAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateOnly dob)
        {
            return dob <= DateOnly.FromDateTime(DateTime.UtcNow);
        }
        return true; // ignore nulls (let [Required] handle it)
    }
}
