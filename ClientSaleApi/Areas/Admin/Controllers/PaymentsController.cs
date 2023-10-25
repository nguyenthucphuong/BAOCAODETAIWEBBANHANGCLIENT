using ClientSaleApi.Models.Payments;
using ClientSaleApi.Models.Roles;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("thanh-toan")]
    public class PaymentsController : BaseController<PaymentEx, ListViewModel<PaymentEx>, InputPayment>
    {

        private readonly HttpClient _httpClient;

        public PaymentsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        [Route("danh-sach")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/Payments/danh-sach-tim-kiem";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }
        [HttpPost]
        [Route("cap-nhat-trang-thai/{paymentId}")]
        public async Task<IActionResult> CapNhatTrangThai(string paymentId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/Payments/cap-nhat-trang-thai/{paymentId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{paymentId}")]
        public async Task<IActionResult> Xoa(string paymentId)
        {
            string baseUrl = $"http://localhost:5260/api/Payments/xoa/{paymentId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }


        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputPayment input)
        {
            string baseUrl = $"http://localhost:5260/api/Payments/them";
            var payment = new PaymentEx(input);
            var result = await base.Them(baseUrl, payment);
            return result;
        }


        [HttpPost]
        [Route("cap-nhat/{paymentId}")]
        public async Task<IActionResult> CapNhat(InputPayment input)
        {
            string paymentId = input.PaymentId!;
            string baseUrl = $"http://localhost:5260/api/Payments/cap-nhat/{paymentId}";
            var payment = new PaymentEx(input);
            var result = await base.CapNhat(baseUrl, payment);
            return result;
        }
  
        [Route("thong-tin/{paymentId}")]
        public async Task<PaymentEx?> GetThongTinPaymentAsync(string paymentId)
        {
            string baseUrl = $"http://localhost:5260/api/Payments/thong-tin/{paymentId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? paymentId)
        {
            if (paymentId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinPaymentAsync(paymentId);
                return View(role);
            }
        }

    }
}
