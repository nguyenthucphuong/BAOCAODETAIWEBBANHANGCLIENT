using ClientSaleApi.Models.Customers;
using ClientSaleApi.Models.Users;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	[Route("khach-hang")]
	public class CustomersController : BaseController<CustomerEx, ListViewModel<CustomerEx>, InputCustomer>
	{

		private readonly HttpClient _httpClient;

		public CustomersController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
		}


		[Route("danh-dach")]
		public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
		{
			var baseUrl = "http://localhost:5260/api/Customers/danh-sach-tim-kiem";
			var result = await base.Index(baseUrl, searchModel, page, message);
			return result;
		}
		[HttpPost]
		[Route("cap-nhat-trang-thai/{customerId}")]
		public async Task<IActionResult> CapNhatTrangThai(string customerId, int page, int maxResultCount)
		{
			string baseUrl = $"http://localhost:5260/api/Customers/cap-nhat-trang-thai/{customerId}";
			var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
			return result;
		}
	}
}
