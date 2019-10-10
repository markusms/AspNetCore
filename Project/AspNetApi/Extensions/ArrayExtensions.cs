using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Extensions
{
    public static class ArrayExtensions
    {
        public static int NullableCount<T>(this IEnumerable<T> collection)
        {
            return collection == null ? 0 : collection.Count();
        }
    }
}
