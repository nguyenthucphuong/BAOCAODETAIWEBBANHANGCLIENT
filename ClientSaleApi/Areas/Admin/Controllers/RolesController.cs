using ClientSaleApi.Controllers;
using ClientSaleApi.Models.Roles;
using ClientSaleApi.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Drawing.Printing;
using X.PagedList;
using System.Text.Json;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Rewrite;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("vai-tro")]
	public class RolesController : BaseController<RoleEx, ListViewModel<RoleEx>, InputRole>
	{

		private readonly HttpClient _httpClient;

		public RolesController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
		}

		[Route("danh-sach")]
		public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
		{
            var baseUrl = "http://localhost:5260/api/Roles/danh-sach-tim-kiem";
			var result = await base.Index(baseUrl, searchModel, page, message);
			return result;
		}
		[HttpPost]
		[Route("cap-nhat-trang-thai/{roleId}")]
		public async Task<IActionResult> CapNhatTrangThai(string roleId, int page, int maxResultCount)
		{
			string baseUrl = $"http://localhost:5260/api/Roles/cap-nhat-trang-thai/{roleId}";
			var result = await base.CapNhatTrangThaiId(baseUrl, page, maxResultCount);
			return result;
		}

		[Route("xoa/{roleId}")]
		public async Task<IActionResult> Xoa(string roleId)
		{
			string baseUrl = $"http://localhost:5260/api/Roles/xoa/{roleId}";
			var result = await base.XoaId(baseUrl);
			return result;
		}


		[HttpPost]
		[Route("them")]
		public async Task<IActionResult> Them(InputRole input)
		{
			string baseUrl = $"http://localhost:5260/api/Roles/them";
			var role = new RoleEx(input);
			var result = await base.Them(baseUrl, role);
			return result;
		}


		[HttpPost]
		[Route("cap-nhat/{roleId}")]
		public async Task<IActionResult> CapNhat(InputRole input)
		{
			string roleId = input.RoleId!;
			string baseUrl = $"http://localhost:5260/api/Roles/cap-nhat/{roleId}";
			var role = new RoleEx(input);
			var result = await base.CapNhat(baseUrl, role);
			return result;
		}

		[Route("thong-tin/{roleId}")]
		public async Task<RoleEx?> GetThongTinRoleAsync(string roleId)
		{
			string baseUrl = $"http://localhost:5260/api/Roles/thong-tin/{roleId}";
			var result = await base.GetThongTinModelAsync(baseUrl);
			return result;
		}
		[Route("edit")]
		public async Task<IActionResult> Edit(string? roleId)
		{
			if (roleId == null)
			{
				return View();
			}
			else
			{
				var role = await GetThongTinRoleAsync(roleId);
				return View(role);
			}
		}

	}


}
