using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.ViewModels.Shops
{
    public class InputShop: SearchViewModel
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? Page { get; set; }
    }
}
