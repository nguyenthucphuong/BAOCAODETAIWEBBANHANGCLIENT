using ClientSaleApi.Models.Carts;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientSaleApi.Models.Components
{
	public class CartQuantityViewComponent : ViewComponent
	{
		private readonly HttpClient _httpClient;

		public CartQuantityViewComponent(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var baseUrl = $"http://localhost:5260/api/Carts/so-luong-gio-hang";
			var response = await _httpClient.GetAsync(baseUrl);
			var quantity = 0;
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				quantity = JsonConvert.DeserializeObject<int>(json);
			}
			return View(quantity);
		}
	}
}
