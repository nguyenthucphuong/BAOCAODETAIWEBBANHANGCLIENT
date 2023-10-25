using Microsoft.AspNetCore.Mvc;

namespace ClientSaleApi.Controllers
{
    [Route("lien-he")]
    public class ContactsController : Controller
	{
		public IActionResult Index()
		{
            return View();
		}
	}
}
