using ClientSaleApi.Models;
using ClientSaleApi.Models.Accounts;
using ClientSaleApi.Models.Products;
using ClientSaleApi.Models.ViewModels;
using ClientSaleApi.Models.ViewModels.Shops;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ClientSaleApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly HttpClient _httpClient;
		public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClient = httpClientFactory.CreateClient("Client");
		}

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeIndexViewModel();
            var baseUrl = "http://localhost:5260/api/Products/danh-sach-san-pham";
            var newProductsResponse = await _httpClient.GetAsync(baseUrl + "?Sorting=isNew");
            if (newProductsResponse.IsSuccessStatusCode)
            {
                var responseContent = await newProductsResponse.Content.ReadAsStringAsync() ?? "";
                viewModel.NewProducts = JsonConvert.DeserializeObject<ListViewModel<OutputShop>>(responseContent) ?? new ListViewModel<OutputShop>();
            }

            var saleProductsResponse = await _httpClient.GetAsync(baseUrl + "?Sorting=isSale");
            if (saleProductsResponse.IsSuccessStatusCode)
            {
                var responseContent = await saleProductsResponse.Content.ReadAsStringAsync() ?? "";
                viewModel.SaleProducts = JsonConvert.DeserializeObject<ListViewModel<OutputShop>>(responseContent) ?? new ListViewModel<OutputShop>();
            }

            var proProductsResponse = await _httpClient.GetAsync(baseUrl + "?Sorting=isPro");
            if (proProductsResponse.IsSuccessStatusCode)
            {
                var responseContent = await proProductsResponse.Content.ReadAsStringAsync() ?? "";
                viewModel.ProProducts = JsonConvert.DeserializeObject<ListViewModel<OutputShop>>(responseContent) ?? new ListViewModel<OutputShop>();
            }
            System.Threading.Thread.Sleep(1000);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}