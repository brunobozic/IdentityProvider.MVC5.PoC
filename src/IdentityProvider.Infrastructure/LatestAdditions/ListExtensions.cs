using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityProvider.Infrastructure.LatestAdditions
{
    public static class ListExtensions
    {
        public static List<Guid> ParseString(string input, char separator = ',')
        {
            return input.Split(separator).Select(x => new Guid(x)).ToList();
        }

        public static List<T> Swap<T>(this List<T> list, int firstIndex, int secondIndex)
        {
            if (firstIndex == secondIndex || firstIndex < 0 || secondIndex < 0)
            {
                return list;
            }

            T temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = temp;

            return list;
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;

            foreach (var item in items)
            {
                if (predicate(item)) return retVal;

                retVal++;
            }

            return -1;
        }
        ///<summary>Finds the index of the first occurence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}
