using ClientSaleApi.Models.Categories;
using ClientSaleApi.Models.Products;

namespace ClientSaleApi.Models.ViewModels.Shops
{
    public class ListDanhMuc
    {
        public List<CategoryEx> Items { get; set; } = new List<CategoryEx>();
    }
}
