using ClientSaleApi.Models.Accounts;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using ClientSaleApi.Models.Users;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("thanh-vien")]
	public class AccountsController : BaseController<UserEx, ListViewModel<UserEx>, InputLogin>
	{
		private readonly HttpClient _httpClient;
		public AccountsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
		}

		[Route("dang-ki")]
		public IActionResult Register(string? message)
		{
			var registerModel = new InputRegister { Message = message };
			return View(registerModel);
		}
		[HttpPost]
		[Route("dang-ki")]
		public async Task<IActionResult> Register(InputRegister input)
		{
			string baseUrl = "http://localhost:5260/api/Accounts/dang-ki";
			if (ModelState.IsValid)
			{
				var form = new Dictionary<string, string>
				{
					{ "UserName", input.UserName },
					{ "Email", input.Email },
					{ "Password",  EncryptPasswordSHA256(input.Password) }
				};
				var content = new FormUrlEncodedContent(form);
				var response = await _httpClient.PostAsync(baseUrl, content);
				var result = await response.Content.ReadAsAsync<ApiResponse>();
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Login", "Accounts", new { message = result?.Message ?? string.Empty });
				}
				else
				{
					return RedirectToAction("Register", "Accounts", new { message = result?.Message ?? string.Empty });
				}
			}
			return View(input);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("dang-nhap")]
		public IActionResult Login(string? message)
		{
			var loginModel = new InputLogin { Message = message };
			return View(loginModel);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("dang-nhap")]
		public async Task<IActionResult> Login(InputLogin input)
		{
			string baseUrl = "http://localhost:5260/api/Accounts/dang-nhap";
			HttpResponseMessage response = new HttpResponseMessage();
			if (string.IsNullOrEmpty(input.Email) && string.IsNullOrEmpty(input.Password))
			{
				response = await _httpClient.GetAsync(baseUrl);
			}
			else if (ModelState.IsValid)
			{

				using var content = new MultipartFormDataContent();
				content.Add(new StringContent(input.Email), "Email");
				content.Add(new StringContent(EncryptPasswordSHA256(input.Password)), "Password");
				response = await _httpClient.PostAsync(baseUrl, content);
			}
			if (response.IsSuccessStatusCode)
			{
				var loginResult = await response.Content.ReadAsAsync<LoginResult>();
				Response.Cookies.Append("UserName", loginResult.UserName);
				Response.Cookies.Append("Token", loginResult.Token);
				return await LoginWithRole(loginResult);
			}
			else
			{
				var result = await response.Content.ReadAsAsync<ApiResponse>();
				return RedirectToAction("Login", "Accounts", new { message = result?.Message ?? string.Empty });
			}
		}

		[AllowAnonymous]
		[Route("dang-nhap-role")]
		public async Task<IActionResult> LoginWithRole(LoginResult loginResult)
		{
			var role = loginResult.RoleName;
			var claims = new[] { new Claim(ClaimTypes.Role, role) };
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
			if (role == "Customer" || role == "Default")
			{
				return RedirectToAction("Index", "Home", new { area = "" });
			}
			else if (role == "User")
			{
				return RedirectToAction("Index", "Home", new { area = "User" });
			}
			else if (role == "Admin")
			{
				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}
			return RedirectToAction("Index", "Home");
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("404")]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		[HttpPost]
		[Route("dang-xuat")]
		public async Task<IActionResult> Logout()
		{
			string baseUrl = "http://localhost:5260/api/Accounts/dang-xuat";

			if (Request.Cookies.TryGetValue("UserName", out var userName))
			{
				var response = await _httpClient.PostAsync(baseUrl, null);
				if (response.IsSuccessStatusCode)
				{
					Response.Cookies.Delete("Token");
					Response.Cookies.Delete("UserName");
					if (userName == "default")
					{
						return RedirectToAction("Index", "Home", new { area = "" });
					}
					else
					{
						return RedirectToAction("Login", "Accounts", new { area = "Admin" });
					}
				}
				else
				{
					return BadRequest("Lỗi đăng xuất!");
				}
			}
			return RedirectToAction("Login", "Accounts", new { area = "Admin" });
		}

		[HttpGet]
		[Route("kiem-tra-token")]
		public IActionResult CheckToken()
		{
			Request.Cookies.TryGetValue("Token", out var token);
			JwtSecurityToken jwtSecurityToken;
			try
			{
				jwtSecurityToken = new JwtSecurityToken(token);
			}
			catch (Exception)
			{
				return Json(new { valid = false });
			}
			return Json(new { valid = jwtSecurityToken.ValidTo > DateTime.UtcNow });
		}


		[HttpGet]
		[Route("kiem-tra-email")]
		public IActionResult CheckEmail(string? message)
		{
			var model = new InputCheckEmail { Message = message };
			return View(model);
		}

		[HttpPost]
		[Route("kiem-tra-email")]
		public async Task<IActionResult> CheckEmail(InputCheckEmail input)
		{
			string baseUrl = "http://localhost:5260/api/Accounts/kiem-tra-email";
			if (ModelState.IsValid)
			{
				var form = new Dictionary<string, string> { { "Email", input.Email } };
				var content = new FormUrlEncodedContent(form);
				var response = await _httpClient.PostAsync(baseUrl, content);
				var result = await response.Content.ReadAsAsync<ApiResponse>();
				if (response.IsSuccessStatusCode)
				{
					input.Message = result?.Message ?? string.Empty;
					return RedirectToAction("ChangePassword", "Accounts", new { message = input.Message, email = input.Email });
				}
				else
				{
					return RedirectToAction("CheckEmail", "Accounts", new { message = result?.Message ?? string.Empty });
				}
			}
			return View();
		}

		[HttpGet]
		[Route("doi-mat-khau")]
		public IActionResult ChangePassword(string message, string email)
		{
			var input = new InputChangePassword
			{
				Message = message,
				Email = email
			};
			return View(input);
		}

		[HttpPost]
		[Route("doi-mat-khau")]
		public async Task<IActionResult> ChangePassword(InputChangePassword input)
		{
			string baseUrl = "http://localhost:5260/api/Accounts/doi-mat-khau";
			if (ModelState.IsValid)
			{
				var form = new Dictionary<string, string>
				{
					{ "Email", input.Email },
					{ "Password", EncryptPasswordSHA256(input.Password) }
				};
				var content = new FormUrlEncodedContent(form);
				var response = await _httpClient.PostAsync(baseUrl, content);
				var result = await response.Content.ReadAsAsync<ApiResponse>();
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Login", "Accounts", new { message = result?.Message ?? string.Empty });
				}
				else
				{
					return RedirectToAction("Register", "Accounts", new { message = result?.Message ?? string.Empty });
				}
			}
			return View(input);
		}
	}
}
