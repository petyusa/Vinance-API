using System;
using System.Collections.Generic;
using System.Linq;

namespace Vinance.Logic
{
    public class PagedList<T>
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; } = new List<T>();

        public PagedList(IEnumerable<T> items, int page, int pageSize)
        {
            var enumerable = items.ToList();
            Items.AddRange(enumerable.Skip((page - 1) * pageSize).Take(pageSize));
            TotalPages = (int)Math.Ceiling(enumerable.Count() / (double)pageSize);
            Page = page;
            PageSize = pageSize;
        }
    }
}