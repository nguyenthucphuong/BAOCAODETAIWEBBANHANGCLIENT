using ClientSaleApi.Models.Categories;
using ClientSaleApi.Models.ViewModels;
using ClientSaleApi.Models.ViewModels.Shops;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientSaleApi.Models.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
		private readonly HttpClient _httpClient;
        public CategoryListViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }
        public async Task<IViewComponentResult> InvokeAsync()
		{
            var baseUrl = $"http://localhost:5260/api/Categories/danh-sach?isActive=true";
            var response = await _httpClient.GetFromJsonAsync<SimpleListViewModel<CategoryEx>>(baseUrl);
			return View(response?.Items);
		}

	}
}
