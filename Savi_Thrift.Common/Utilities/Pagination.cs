namespace Savi_Thrift.Common.Utilities
{
    public class Pagination
    {

        public async Task<PageResult<T>> PaginateAsync<T>(
            IEnumerable<T> data, Func<T, string> nameSelector, Func<T, int> idSelector, int page, int perPage)
        {
            perPage = perPage <= 0 ? 10 : perPage;
            page = page <= 0 ? 1 : page;


            var orderedData = OrderData(data, nameSelector, idSelector);
            var totalData = orderedData.Count();
            var totalPagedCount = CalculateTotalPages(totalData, perPage);
            var pagedData = GetPagedData(orderedData, page, perPage);

            return new PageResult<T>
            {
                Data = pagedData,
                TotalPageCount = totalPagedCount,
                CurrentPage = page,
                PerPage = pagedData.Count(),
                TotalCount = totalData
            };
        }

        private IOrderedEnumerable<T> OrderData<T>(IEnumerable<T> data, Func<T, string> nameSelector, Func<T, int> idSelector)
        {
            return data.OrderBy(nameSelector).ThenBy(idSelector);
        }

        private int CalculateTotalPages(int totalData, int perPage)
        {
            return (int)Math.Ceiling((double)totalData / perPage);
        }

        private IEnumerable<T> GetPagedData<T>(IOrderedEnumerable<T> orderedData, int page, int perPage)
        {
            return orderedData.Skip((page - 1) * perPage).Take(perPage);
        }
    }
}

