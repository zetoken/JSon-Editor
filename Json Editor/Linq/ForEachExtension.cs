using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTn.Json.Editor.Linq
{
    public static class ForEachExtension
    {
        /// <summary>
        /// Immediatly execute the <paramref name="action"/> on each element of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource element in source)
            {
                action(element);
            }
        }
    }
}
