using ClientSaleApi.Models.ViewModels;

namespace ClientSaleApi.Models.Orders
{
	public class OrderResponse : ApiResponse
	{
		public ListViewModel<OrderEx> Orders { get; set; } = new ListViewModel<OrderEx>();

    }

}
