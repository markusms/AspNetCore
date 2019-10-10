using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetApi.Models;

namespace AspNetApi.Validation
{
    public class DateIsNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var newRun = (NewRun)validationContext.ObjectInstance;
            if (DateTime.Now.Subtract(newRun.TimePosted).TotalSeconds < 0)
                return new ValidationResult("Date is in the future!");
            return ValidationResult.Success;
        }
    }
}
