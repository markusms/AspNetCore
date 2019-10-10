using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    /// <summary>
    /// Class used for fetching leaderboard information
    /// </summary>
    public class RunFilter
    {
        public string Level { get; set; }
        public DateFilter DateFilter { get; set; }
        public int amountOfItems { get; set; }
    }
}
