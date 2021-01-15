using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Polkadot.Utils
{
    public static class EnumerableExtensions
    {
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as ICollection<T> ?? enumerable.ToList();
        }
    }
}