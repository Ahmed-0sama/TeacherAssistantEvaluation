using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validation
{
    public class PastDateOnlyAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date > DateOnly.FromDateTime(DateTime.Today))
                {
                    return new ValidationResult("التاريخ يجب أن يكون في الماضي");
                }
            }

            return ValidationResult.Success;
        }
    }
}
