using ClientSaleApi.Models.OrderStatuses;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("trang-thai-don-hang")]
    public class OrderStatusesController : BaseController<OrderStatusEx, ListViewModel<OrderStatusEx>, InputOrderStatus>
    {

        private readonly HttpClient _httpClient;

        public OrderStatusesController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        [Route("danh-sach")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/OrderStatuses/danh-sach-tim-kiem";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }
        [HttpPost]
        [Route("cap-nhat-trang-thai/{orderStatusId}")]
        public async Task<IActionResult> CapNhatTrangThai(string orderStatusId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/OrderStatuses/cap-nhat-trang-thai/{orderStatusId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{orderStatusId}")]
        public async Task<IActionResult> Xoa(string orderStatusId)
        {
            string baseUrl = $"http://localhost:5260/api/OrderStatuses/xoa/{orderStatusId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }


        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputOrderStatus input)
        {
            string baseUrl = $"http://localhost:5260/api/OrderStatuses/them";
            var orderStatus = new OrderStatusEx(input);
            var result = await base.Them(baseUrl, orderStatus);
            return result;
        }


        [HttpPost]
        [Route("cap-nhat/{orderStatusId}")]
        public async Task<IActionResult> CapNhat(InputOrderStatus input)
        {
            string orderStatusId = input.OrderStatusId!;
            string baseUrl = $"http://localhost:5260/api/OrderStatuses/cap-nhat/{orderStatusId}";
            var orderStatus = new OrderStatusEx(input);
            var result = await base.CapNhat(baseUrl, orderStatus);
            return result;
        }

        [Route("thong-tin/{orderStatusId}")]
        public async Task<OrderStatusEx?> GetThongTinOrderStatusAsync(string orderStatusId)
        {
            string baseUrl = $"http://localhost:5260/api/OrderStatuses/thong-tin/{orderStatusId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? orderStatusId)
        {
            if (orderStatusId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinOrderStatusAsync(orderStatusId);
                return View(role);
            }
        }

    }
}
