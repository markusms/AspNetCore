using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetApi.Validation;

namespace AspNetApi.Models
{
    public class NewRun : IRun
    {
        [Required]
        [Range(0, 99999)]
        public float TimeTaken { get; set; } //speedrun time

        [Required]
        [DateIsNotInFuture]
        public DateTime TimePosted { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Level { get; set; }
    }
}
