using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityProvider.Infrastructure.LatestAdditions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        ///     Join elements of IEnumerable with string separating them
        /// </summary>
        public static string Join<TSource>(this IEnumerable<TSource> source, string on)
        {
            return source.Aggregate(string.Empty,
                (current, item) => current == string.Empty ? item.ToString() : $"{current}{on}{item}");
        }

        public static bool In<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        public static bool NotIn<T>(this T item, params T[] list)
        {
            return !list.Contains(item);
        }

        public static IEnumerable<Tuple<T1, T2>> FullOuterJoin<T1, T2>(this IEnumerable<T1> one, IEnumerable<T2> two,
            Func<T1, T2, bool> match)
        {
            var left = from a in one
                from b in two.Where(b => match(a, b)).DefaultIfEmpty()
                select new Tuple<T1, T2>(a, b);

            var right = from b in two
                from a in one.Where(a => match(a, b)).DefaultIfEmpty()
                select new Tuple<T1, T2>(a, b);

            return left.Concat(right).Distinct();
        }
    }
}