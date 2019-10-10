using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class DateIsHistoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var newItem = (NewItem)validationContext.ObjectInstance;
            if (DateTime.Now.Subtract(newItem.CreationTime).TotalSeconds < 0)
                return new ValidationResult("Date is in the future!");
            return ValidationResult.Success;
        }
    }
}
