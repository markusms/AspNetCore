using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    public class PlayerInformation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsBanned { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreationTime { get; set; }
        public Run[] Runs { get; set; }
    }
}
