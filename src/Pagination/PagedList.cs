namespace AntiRap.Core.Pagination
{
    using AntiRap.Core.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageSize, int pageNumber)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public static PagedList<T> ToPagedList<T>(IQueryable<T> dataSource, int pageSize, int pageNumber)
            where T : class
        {
            int count = dataSource.Count();
            List<T> items = dataSource.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageSize, pageNumber);
        }
    }
}
