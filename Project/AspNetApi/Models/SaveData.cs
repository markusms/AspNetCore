using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    public class SaveData
    {
        [Required]
        [Range(0, 999)]
        public int LemmingsSavedTotal { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string CurrentLevel { get; set; }
        [Required]
        [Range(0, 99999)]
        public float PlayTime { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")] //only the alphabet is allowed (1-20 chars)
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Password { get; set; }
    }
}
