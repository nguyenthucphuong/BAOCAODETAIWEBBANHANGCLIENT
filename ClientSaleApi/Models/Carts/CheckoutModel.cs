using ClientSaleApi.Models.Customers;
using ClientSaleApi.Models.ViewModels;

namespace ClientSaleApi.Models.Carts
{
    public class CheckoutModel
	{
        public ListViewModel<CartEx> Cart { get; set; } = new ListViewModel<CartEx>();
        public CustomerEx Customer { get; set; } = new CustomerEx();
        public string ClientToken { get; set; } = string.Empty;
	}
}
