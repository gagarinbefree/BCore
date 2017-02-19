using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.Extensions
{
    public static class Common
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action.Invoke(enumerator.Current);
            }
        }
    }
}
