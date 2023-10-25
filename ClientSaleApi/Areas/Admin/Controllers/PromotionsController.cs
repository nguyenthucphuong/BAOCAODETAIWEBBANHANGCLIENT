using ClientSaleApi.Models.Promotions;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("khuyen-mai")]
    public class PromotionsController : BaseController<PromotionEx, ListViewModel<PromotionEx>, InputPromotion>
    {
        private readonly HttpClient _httpClient;

        public PromotionsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }


        [Route("danh-sach")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/Promotions/danh-sach-tim-kiem";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }
        [HttpPost]
        [Route("cap-nhat-trang-thai/{promotionId}")]
        public async Task<IActionResult> CapNhatTrangThai(string promotionId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/Promotions/cap-nhat-trang-thai/{promotionId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{promotionId}")]
        public async Task<IActionResult> Xoa(string promotionId)
        {
            string baseUrl = $"http://localhost:5260/api/Promotions/xoa/{promotionId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }


        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputPromotion input)
        {
            string baseUrl = $"http://localhost:5260/api/Promotions/them";
            var promotion = new PromotionEx(input);
            var result = await base.Them(baseUrl, promotion);
            return result;
        }


        [HttpPost]
        [Route("cap-nhat/{promotionId}")]
        public async Task<IActionResult> CapNhat(InputPromotion input)
        {
            string promotionId = input.PromotionId!;
            string baseUrl = $"http://localhost:5260/api/Promotions/cap-nhat/{promotionId}";
            var promotion = new PromotionEx(input);
            var result = await base.CapNhat(baseUrl, promotion);
            return result;
        }

        [Route("thong-tin/{promotionId}")]
        public async Task<PromotionEx?> GetThongTinPromotionAsync(string promotionId)
        {
            string baseUrl = $"http://localhost:5260/api/Promotions/thong-tin/{promotionId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? promotionId)
        {
            if (promotionId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinPromotionAsync(promotionId);
                return View(role);
            }
        }

    }
}
