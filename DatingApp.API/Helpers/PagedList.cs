using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        private PagedList(List<T> items,int count,int pageNum, int pageSize)
        {
            PageSize = pageSize;
            TotalCount = count;
            CurrentPage = pageNum;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            this.AddRange(items);

        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> Source,int pageNum
            ,int pageSize)
        {
            var count = await Source.CountAsync();
            var items = await Source.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNum, pageSize);
        }
    }
}
