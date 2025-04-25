using System.ComponentModel.DataAnnotations;

namespace EmployeersManager.Infrastructure.CustomValidations;

public static class DateValidator
{
    public static ValidationResult ValidateHireDate(DateTime date, ValidationContext context)
    {
        if (date > DateTime.Today)
        {
            return new ValidationResult("Дата прийняття не може бути у майбутньому");
        }

        return ValidationResult.Success;
    }
}
