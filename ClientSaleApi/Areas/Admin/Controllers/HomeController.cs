using ClientSaleApi.Models.Authentication;
using ClientSaleApi.Models.Orders;
using ClientSaleApi.Models.Reports;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	[Route("admin")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

	}
}

