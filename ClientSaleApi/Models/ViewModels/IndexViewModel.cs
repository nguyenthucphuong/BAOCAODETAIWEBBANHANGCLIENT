using X.PagedList;

namespace ClientSaleApi.Models.ViewModels
{
    public class IndexViewModel<T>: ApiResponse
    {
        public StaticPagedList<T>? Items { get; set; }
        public string? Search { get; set; }
        public PagingViewModel? PagingModel { get; set; }

    }
}
