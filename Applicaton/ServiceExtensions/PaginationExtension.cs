using Apposite.Core.Pagination;

namespace Apposite.Application.ServiceExtensions
{
    public static class PaginationExtension
    {
        public static IQueryable<T> ApplyPaginationQueryable<T>(
            this IQueryable<T> query,
            PaginationFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "PaginationFilter cannot be null.");
            }

            var pagedQuery = query;

            var pageNumber = filter.PageNumber;
            var pageSize = filter.PageSize;

            pagedQuery = pagedQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return pagedQuery;
        }


    }
}
