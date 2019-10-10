using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    public interface IRun
    {
        float TimeTaken { get; }
        DateTime TimePosted { get; }
        string Level { get; }
    }
}
