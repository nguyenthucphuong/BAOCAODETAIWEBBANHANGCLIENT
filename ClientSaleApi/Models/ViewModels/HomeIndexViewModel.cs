using ClientSaleApi.Models.Products;
using ClientSaleApi.Models.ViewModels.Shops;

namespace ClientSaleApi.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public ListViewModel<OutputShop> NewProducts { get; set; } = new ListViewModel<OutputShop>();
        public ListViewModel<OutputShop> SaleProducts { get; set; } = new ListViewModel<OutputShop>();
        public ListViewModel<OutputShop> ProProducts { get; set; } = new ListViewModel<OutputShop>();
    }
}
