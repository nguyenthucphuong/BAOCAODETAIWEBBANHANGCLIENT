using ClientSaleApi.Models.ViewModels;

namespace ClientSaleApi.Models.OrderItems
{
	public class OrderItemsViewModel
	{
		public int DiscountCode { get; set; }
		public string PromotionName { get; set; } = string.Empty;
		public int DiscountValue { get; set; }
		public ListViewModel<OrderItemEx> OrderItems { get; set; } = new ListViewModel<OrderItemEx>();
	}
}
