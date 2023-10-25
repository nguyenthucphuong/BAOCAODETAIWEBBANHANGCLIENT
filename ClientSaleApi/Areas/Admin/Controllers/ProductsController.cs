using ClientSaleApi.Models.Authentication;
using ClientSaleApi.Models.Categories;
using ClientSaleApi.Models.Products;
using ClientSaleApi.Models.Roles;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("san-pham")]
    public class ProductsController : BaseController<ProductEx, ListViewModel<ProductEx>, InputProduct>
    {
        private readonly HttpClient _httpClient;

        public ProductsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }



        [Route("danh-sach")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/Products/danh-sach-tim-kiem";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }


		[Route("danh-sach-category")]
		public async Task<List<string>> DanhSachCategory()
		{
			string baseUrl = "http://localhost:5260/api/Categories/danh-sach-ten?isActive=true";
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<List<string>>();
				return result;
			}
			return new List<string>();
		}

		[Route("danh-sach-promotion")]
		public async Task<List<string>> DanhSachPromotion()
		{
			string baseUrl = "http://localhost:5260/api/Promotions/danh-sach-ten?isActive=true";
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<List<string>>();
				return result;
			}
			return new List<string>();
		}

		[HttpPost]
        [Route("cap-nhat-trang-thai/{productId}")]
        public async Task<IActionResult> CapNhatTrangThai(string productId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/Products/cap-nhat-trang-thai/{productId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{productId}")]
        public async Task<IActionResult> Xoa(string productId)
        {
            string baseUrl = $"http://localhost:5260/api/Products/xoa/{productId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }

        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputProduct input)
        {
            string baseUrl = $"http://localhost:5260/api/Products/them";
            var product = new ProductEx(input);
            var result = await base.Them(baseUrl, product);
            return result;
        }

        [HttpPost]
        [Route("cap-nhat/{productId}")]
        public async Task<IActionResult> CapNhat(InputProduct input)
        {
            string productId = input.ProductId!;
            string baseUrl = $"http://localhost:5260/api/Products/cap-nhat/{productId}";
            var product = new ProductEx(input);
            var result = await base.CapNhat(baseUrl, product);
            return result;
        }

        [Route("thong-tin/{productId}")]
        public async Task<ProductEx?> GetThongTinProductAsync(string productId)
        {
            string baseUrl = $"http://localhost:5260/api/Products/thong-tin/{productId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? productId)
        {
            if (productId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinProductAsync(productId);
                return View(role);
            }
        }

    }
}
