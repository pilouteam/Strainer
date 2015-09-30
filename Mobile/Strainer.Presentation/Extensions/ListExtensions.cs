using System;
using System.Collections.Generic;

namespace Strainer.Extensions
{
    public static class ListExtensions
    {
        public static void ForEach<T>(this List<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T element in source)
			{
				action(element);
			}
		}
    }
}