using ClientSaleApi.Models.OrderItems;
using ClientSaleApi.Models.ViewModels;

namespace ClientSaleApi.Models.Orders
{
	public class OrderHistoryViewModel
	{
		public ListViewModel<OrderEx> Orders { get; set; } = new ListViewModel<OrderEx>();
		public ListViewModel<OrderItemEx> OrderItems { get; set; } = new ListViewModel<OrderItemEx>();
	}
}
