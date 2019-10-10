using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    public enum DateFilter
    {
        [EnumMember(Value = "allTime")]
        allTime,
        [EnumMember(Value = "today")]
        today,
        [EnumMember(Value = "week")]
        week
    }
}
