using ClientSaleApi.Models.Categories;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("danh-muc")]
    public class CategoriesController : BaseController<CategoryEx, ListViewModel<CategoryEx>, InputCategory>
    {

        private readonly HttpClient _httpClient;

        public CategoriesController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

		[Route("danh-sach")]
		public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
		{
			var baseUrl = "http://localhost:5260/api/Categories/danh-sach-tim-kiem";
			var result = await base.Index(baseUrl, searchModel, page, message);
			return result;
		}
		[HttpPost]
        [Route("cap-nhat-trang-thai/{categoryId}")]
        public async Task<IActionResult> CapNhatTrangThai(string categoryId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/Categories/cap-nhat-trang-thai/{categoryId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{categoryId}")]
        public async Task<IActionResult> Xoa(string categoryId)
        {
            string baseUrl = $"http://localhost:5260/api/Categories/xoa/{categoryId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }


        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputCategory input)
        {
            string baseUrl = $"http://localhost:5260/api/Categories/them";
            var category = new CategoryEx(input);
            var result = await base.Them(baseUrl, category);
            return result;
        }


        [HttpPost]
        [Route("cap-nhat/{categoryId}")]
        public async Task<IActionResult> CapNhat(InputCategory input)
        {
            string categoryId = input.CategoryId!;
            string baseUrl = $"http://localhost:5260/api/Categories/cap-nhat/{categoryId}";
            var category = new CategoryEx(input);
            var result = await base.CapNhat(baseUrl, category);
            return result;
        }

        [Route("thong-tin/{categoryId}")]
        public async Task<CategoryEx?> GetThongTinCategoryAsync(string categoryId)
        {
            string baseUrl = $"http://localhost:5260/api/Categories/thong-tin/{categoryId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? categoryId)
        {
            if (categoryId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinCategoryAsync(categoryId);
                return View(role);
            }
        }

    }
}
