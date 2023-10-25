namespace ClientSaleApi.Models.ViewModels.Shops
{
    public class ProductListViewModel : SearchViewModel
    {
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public int? minPrice { get; set; }
        public int? maxPrice { get; set; }
        public string? categoryName { get; set; }

    }
}
