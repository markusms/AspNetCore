using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    public interface IPlayer
    {
        string Name { get; }
        string Password { get; }
        bool IsAdmin { get; }
    }
}
