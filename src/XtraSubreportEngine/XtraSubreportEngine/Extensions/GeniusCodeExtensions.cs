using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeniusCode.Framework.Extensions
{
    public static class GeniusCodeExtensions
    {

        public static IEnumerable<Tuple<T1, T2>> FullOuterJoin<T1, T2>(this IEnumerable<T1> one, IEnumerable<T2> two, Func<T1,T2,bool> match)
        {
            var left = from a in one
                       from b in two.Where((b) => match(a, b)).DefaultIfEmpty()
                       select new Tuple<T1, T2>(a, b);

            var right = from b in two
                        from a in one.Where((a) => match(a, b)).DefaultIfEmpty()
                        select new Tuple<T1, T2>(a, b);

            return left.Union(right);
        }

    }
}
