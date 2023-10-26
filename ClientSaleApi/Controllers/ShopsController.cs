using Azure;
using ClientSaleApi.Areas.Admin.Controllers;
using ClientSaleApi.Models.Accounts;
using ClientSaleApi.Models.Categories;
using ClientSaleApi.Models.Products;
using ClientSaleApi.Models.Roles;
using ClientSaleApi.Models.ViewModels;
using ClientSaleApi.Models.ViewModels.Shops;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Drawing.Printing;
using System.Security.Policy;
using System.Web;
using X.PagedList;

namespace ClientSaleApi.Controllers
{
	public class ShopsController : BaseController<ProductEx, ListViewModel<ProductEx>, ProductListViewModel>
	{

		private readonly HttpClient _httpClient;

		public ShopsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
		}

		[Route("shop/san-pham")]
		public async Task<IActionResult> Index(SearchViewModel searchModel, int? minPrice, int? maxPrice, int? page)
		{
			string baseUrl = $"http://localhost:5260/api/Products/danh-sach-san-pham";
			int pageSize = searchModel.MaxResultCount;
			int pageNumber = page ?? 1;
			searchModel.SkipCount = (pageNumber - 1) * pageSize;
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["SkipCount"] = searchModel.SkipCount.ToString();
			query["MaxResultCount"] = searchModel.MaxResultCount.ToString();
			query["Search"] = searchModel.Search;
			query["Sorting"] = searchModel.Sorting;

			if (minPrice.HasValue)
			{
				query["minPrice"] = minPrice.Value.ToString();
			}
			if (maxPrice.HasValue)
			{
				query["maxPrice"] = maxPrice.Value.ToString();
			}
			var url = $"{baseUrl}?{query}";

			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<ListSanPham>();
				var items = new StaticPagedList<OutputShop>(result.Items, pageNumber, pageSize, result.TotalCount);
				var viewModel = new ShopIndexViewModel
				{
					Items = items,
					Search = searchModel.Search,
					PagingModel = new PagingViewModel
					{
						Page = pageNumber,
						PageSize = pageSize,
						TotalCount = result.TotalCount,
					},
					AllPriceCount = result.AllPriceCount,
					Price1Count = result.Price1Count,
					Price2Count = result.Price2Count,
					Price3Count = result.Price3Count
				};
				System.Threading.Thread.Sleep(1000);
				return View(viewModel);
			}
			return View(new ShopIndexViewModel());
		}

		[Route("chi-tiet")]
		public IActionResult SelectProduct(string productId, string friendlyUrl)
		{
			HttpContext.Session.SetString("ProductId", productId);
			return RedirectToAction("Chitiet", "Shops", new { friendlyUrl = friendlyUrl });
		}
        
        [Route("{friendlyUrl}")]
        public async Task<IActionResult> Chitiet(string friendlyUrl, string? message)
        {
            string productId = HttpContext.Session.GetString("ProductId") ?? string.Empty;
            var result = new OutputProduct();
            string baseUrl = $"http://localhost:5260/api/Products/thong-tin-san-pham/{productId}";
            var response = await _httpClient.GetAsync(baseUrl);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<OutputProduct>();
				result.FriendlyUrl = friendlyUrl;
				result.Message = message;
            }
            System.Threading.Thread.Sleep(1000);
            return View(result);
        }

		[Route("lay-hinh")]
		public async Task<Stream?> GetImage(string path)
		{
			string baseUrl = $"http://localhost:5260/api/Products/GetImage?path={path}";
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStreamAsync();
				return result;
			}
			return null;
		}


	}
}
