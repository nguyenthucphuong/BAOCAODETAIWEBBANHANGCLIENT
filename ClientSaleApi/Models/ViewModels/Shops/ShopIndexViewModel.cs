using ClientSaleApi.Models.Products;

namespace ClientSaleApi.Models.ViewModels.Shops
{
	public class ShopIndexViewModel : IndexViewModel<OutputShop>
	{
		public int AllPriceCount { get; set; }
		public int Price1Count { get; set; }
		public int Price2Count { get; set; }
		public int Price3Count { get; set; }

	}
}
