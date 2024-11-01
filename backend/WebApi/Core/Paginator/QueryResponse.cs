using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq.Dynamic.Core;

namespace WebApi.Core.Paginator
{
    public class QueryResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int? PageSize { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(Count / (decimal)PageSize);
        public int Count { get; set; }
    }

    public static class Paginator
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, QueryParams queryParams, out int count)
        {
            count = query.Count();

            queryParams.Page = !queryParams.Page.HasValue || queryParams.Page < 1 ? 1 : queryParams.Page;
            queryParams.PageSize ??= count;

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                query = query
                    .OrderBy($"{queryParams.OrderBy} {queryParams.Direction}");
            }

            query = query
                .Skip(queryParams.Offset ?? 0)
                .Take(queryParams.PageSize ?? count);

            return query;
        }
    }
}
