using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Strainer.Extensions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> thisEnumerable)
        {
            if(thisEnumerable == null)
                {
                    return new T[0];
                }
            return thisEnumerable;
        }

        public static IEnumerable<object> Safe(this IEnumerable thisEnumerable)
        {
            if(thisEnumerable == null)
                {
                    return new object[0];
                }
            return thisEnumerable.Cast<object>();
        }
    }
}

