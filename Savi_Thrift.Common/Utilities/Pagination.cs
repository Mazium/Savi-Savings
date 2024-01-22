using CloudinaryDotNet.Actions;

namespace Savi_Thrift.Common.Utilities
{
    public class Pagination<T>
    {
        public static async Task<PageResult<IEnumerable<T>>> PaginateAsync(IEnumerable<T> data, Func<T, string> nameSelector,
            Func<T, string> idSelector, int page, int perPage)


        {
            perPage = perPage <= 0 ? 10 : perPage;
            page = page <= 0 ? 1 : page;

            var orderedData = data.OrderBy(item => nameSelector(item)).ThenBy(item => idSelector(item));
            var totalData = orderedData.Count();
            int totalPagedCount = CalculateTotalPages(totalData, perPage);
            var pagedData = GetPagedData(orderedData, page, perPage);


            await Task.Delay(1);
           
            return new PageResult<IEnumerable<T>>
            {
                Data = pagedData,
                TotalPageCount = totalPagedCount,
                CurrentPage = page,
                PerPage = pagedData.Count(),
                TotalCount = totalData
            };
        }


        private static int CalculateTotalPages(int totalData, int perPage)
        {
            return (int)Math.Ceiling((double)totalData / perPage);
        }


        private static IEnumerable<T> GetPagedData<T>(IOrderedEnumerable<T> data, int page, int perPage)
        {
            return data.Skip((page - 1) * perPage).Take(perPage);
        }
    }
    
}

