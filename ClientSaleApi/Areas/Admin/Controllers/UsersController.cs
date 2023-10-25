using ClientSaleApi.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
using ClientSaleApi.Controllers;
using ClientSaleApi.Models.Payments;
using ClientSaleApi.Models.ViewModels;
using System.Security.Cryptography;
using ClientSaleApi.Models.Roles;
using Microsoft.AspNetCore.Authorization;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("tai-khoan")]
    public class UsersController : BaseController<UserEx, ListViewModel<UserEx>, InputUser>
    {
        private readonly HttpClient _httpClient;

        public UsersController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }


        [Route("danh-sach")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/Users/danh-sach-tim-kiem";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }


		[Route("danh-sach-role")]
		public async Task<List<string>> DanhSachRole()
		{
			string baseUrl = "http://localhost:5260/api/Roles/danh-sach-ten?isActive=true";
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<List<string>>();
				return result;
			}
			return new List<string>();
		}


		[HttpPost]
        [Route("cap-nhat-trang-thai/{userId}")]
        public async Task<IActionResult> CapNhatTrangThai(string userId, int page, int maxResultCount)
        {
            string baseUrl = $"http://localhost:5260/api/Users/cap-nhat-trang-thai/{userId}";
            var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
            return result;
        }

        [Route("xoa/{userId}")]
        public async Task<IActionResult> Xoa(string userId)
        {
            string baseUrl = $"http://localhost:5260/api/Users/xoa/{userId}";
            var result = await base.XoaId(baseUrl);
            return result;
        }

        [HttpPost]
        [Route("them")]
        public async Task<IActionResult> Them(InputUser input)
        {
            string baseUrl = $"http://localhost:5260/api/Users/them";
            var user = new UserEx(input);
            user.Password = EncryptPasswordSHA256(input.Password);
            var result = await base.Them(baseUrl, user);
            return result;
        }

        [HttpPost]
        [Route("cap-nhat/{userId}")]
        public async Task<IActionResult> CapNhat(InputUser input)
        {
            string userId = input.UserId!;
            string baseUrl = $"http://localhost:5260/api/Users/cap-nhat/{userId}";
            var user = new UserEx(input);
			user.Password = EncryptPasswordSHA256(input.Password);
			var result = await base.CapNhat(baseUrl, user);
            return result;
        }

        [Route("thong-tin/{userId}")]
        public async Task<UserEx?> GetThongTinUserAsync(string userId)
        {
            string baseUrl = $"http://localhost:5260/api/Users/thong-tin/{userId}";
            var result = await base.GetThongTinModelAsync(baseUrl);
            return result;
        }
        [Route("edit")]
        public async Task<IActionResult> Edit(string? userId)
        {
            if (userId == null)
            {
                return View();
            }
            else
            {
                var role = await GetThongTinUserAsync(userId);
                return View(role);
            }
        }




    }
}
