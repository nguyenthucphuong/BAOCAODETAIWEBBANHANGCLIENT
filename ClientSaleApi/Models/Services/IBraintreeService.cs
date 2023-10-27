using Braintree;

namespace ClientSaleApi.Models.Services
{
	public interface IBraintreeService
	{
		IBraintreeGateway CreateGateway();
		IBraintreeGateway GetGateway();
	}
}
