using System.Collections.Generic;

namespace Vinance.Logic
{
    public static class IEnumerableExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> items, int page, int pageSize)
        {
            return new PagedList<T>(items, page, pageSize);
        }
    }
}