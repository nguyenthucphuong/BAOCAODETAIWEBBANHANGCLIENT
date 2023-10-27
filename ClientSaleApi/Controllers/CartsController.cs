using Braintree;
using ClientSaleApi.Areas.Admin.Controllers;
using ClientSaleApi.Models.Carts;
using ClientSaleApi.Models.Customers;
using ClientSaleApi.Models.OrderItems;
using ClientSaleApi.Models.Orders;
using ClientSaleApi.Models.Payments;
using ClientSaleApi.Models.Promotions;
using ClientSaleApi.Models.Services;
using ClientSaleApi.Models.Users;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static NuGet.Packaging.PackagingConstants;

namespace ClientSaleApi.Controllers
{
	[Route("gio-hang")]
	public class CartsController : BaseController<CartEx, ListViewModel<CartEx>, InputCart>
	{
		private readonly HttpClient _httpClient;
		private readonly IBraintreeService _braintreeService;

		public CartsController(IHttpClientFactory httpClientFactory, IBraintreeService braintreeService) : base(httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("Client");
			_braintreeService = braintreeService;
		}

		[Route("xem-gio-hang")]
		public async Task<IActionResult> Index()
		{
			if (!Request.Cookies.TryGetValue("UserName", out var userName) || userName == "default")
			{
				return RedirectToAction("Login", "Accounts", new { area = "Admin" });
			}
			var baseUrl = $"http://localhost:5260/api/Carts/gio-hang";
			var cart = new ListViewModel<CartEx>();
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				cart = JsonConvert.DeserializeObject<ListViewModel<CartEx>>(json);
			}

			return View(cart);
		}

		[HttpPost]
		[Route("them-gio-hang")]
		public async Task<IActionResult> AddToCart(InputCart input)
		{
			if (!Request.Cookies.TryGetValue("UserName", out var userName) || userName == "default")
			{
				return RedirectToAction("Login", "Accounts", new { area = "Admin" });
			}

			var baseUrl = $"http://localhost:5260/api/Carts/them-gio-hang";
			var json = JsonConvert.SerializeObject(input);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, content);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<ApiResponse>();

				if (result.Message == null)
				{
					return RedirectToAction("Index", "Carts", new { area = "" });
				}
				else
				{
					return RedirectToAction("Chitiet", "Shops", new { area = "", friendlyUrl = input.FriendlyUrl, message = result.Message });
				}
			}

			return View();
		}

		[Route("xoa-item/{productId}")]
		public async Task<IActionResult> XoaItemId(string productId)
		{
			var baseUrl = $"http://localhost:5260/api/Carts/xoa-item/{productId}";
			var response = await _httpClient.DeleteAsync(baseUrl);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Carts", new { area = "" });
			}
			else
			{
				return BadRequest("Không thể xóa sản phẩm!");
			}
		}

		[HttpPost("cap-nhat-so-luong")]
		public async Task<IActionResult> UpdateQuantity(string productId, int changeQuantity)
		{
			var baseUrl = $"http://localhost:5260/api/Carts/cap-nhat-so-luong?productId={productId}&changeQuantity={changeQuantity}";
			var response = await _httpClient.PostAsync(baseUrl, null);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return BadRequest("Không thể cập nhật số lượng sản phẩm!");
			}
		}

		[HttpGet("discount")]
		public async Task<ActionResult<OutputDiscount>> GetDiscount(string promotionName)
		{
			var baseUrl = $"http://localhost:5260/api/Promotions/discount?promotionName={promotionName}";
			var response = await _httpClient.GetAsync(baseUrl);

			if (response.IsSuccessStatusCode)
			{
				var discount = await response.Content.ReadAsAsync<OutputDiscount>();
				return Ok(discount);
			}
			else
			{
				return BadRequest("Không thể lấy giá trị khuyến mãi!");
			}
		}

		[Route("tao-don-hang")]
		public async Task<IActionResult> Checkout()
		{
			if (!Request.Cookies.TryGetValue("UserName", out var userName) || userName == "default")
			{
				return RedirectToAction("Login", "Accounts", new { area = "Admin" });
			}
			var baseUrl = $"http://localhost:5260/api";
			var checkoutModel = new CheckoutModel();

			// Lấy thông tin giỏ hàng
			var cartResponse = await _httpClient.GetAsync($"{baseUrl}/Carts/gio-hang");
			if (cartResponse.IsSuccessStatusCode)
			{
				var cartJson = await cartResponse.Content.ReadAsStringAsync();
				checkoutModel.Cart = JsonConvert.DeserializeObject<ListViewModel<CartEx>>(cartJson)!;
			}
			else
			{
				return BadRequest("Lỗi không thể lấy thông tin giỏ hàng!");
			}

			// Lấy thông tin khách hàng
			var customerResponse = await _httpClient.GetAsync($"{baseUrl}/Customers/khach-hang");
			if (customerResponse.IsSuccessStatusCode)
			{
				var customerJson = await customerResponse.Content.ReadAsStringAsync();
				checkoutModel.Customer = JsonConvert.DeserializeObject<CustomerEx>(customerJson)!;
			}
			else
			{
				return BadRequest("Lỗi không thể lấy thông tin khách hàng!");
			}
			// Tạo token Braintree
			var gateway = _braintreeService.GetGateway();
			checkoutModel.ClientToken = gateway.ClientToken.Generate();
			return View(checkoutModel);
		}

		[Route("pay-braintree")]
		public ActionResult PayBraintree(System.Decimal amount, string nonce, string customerName, string phoneNumber)
		{
			var gateway = _braintreeService.GetGateway();

			// Tạo thông tin khách hàng
			var customerRequest = new CustomerRequest
			{
				FirstName = customerName,
				Phone = phoneNumber
			};
			Result<Customer> customerResult = gateway.Customer.Create(customerRequest);

			if (!customerResult.IsSuccess())
			{
				return Json(new { isSuccess = false, message = "Không thể tạo thông tin khách hàng" });
			}

			// Tạo phương thức thanh toán cho khách hàng
			var paymentMethodRequest = new PaymentMethodRequest
			{
				CustomerId = customerResult.Target.Id,
				PaymentMethodNonce = nonce
			};
			Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(paymentMethodRequest);

			if (!paymentMethodResult.IsSuccess())
			{
				return Json(new { isSuccess = false, message = "Không thể tạo phương thức thanh toán" });
			}

			// Sử dụng token từ kết quả để tạo giao dịch
			var request = new TransactionRequest
			{
				Amount = amount,
				PaymentMethodToken = paymentMethodResult.Target.Token,
				Options = new TransactionOptionsRequest { SubmitForSettlement = true }
			};

			Result<Transaction> result = gateway.Transaction.Sale(request);
			if (result.IsSuccess())
			{
				Transaction transaction = result.Target;
				return Json(new { isSuccess = true, message = transaction.Id });
			}
			else if (result.Transaction != null)
			{
				return Json(new { isSuccess = true, message = result.Transaction.Id });
			}
			else
			{
				string errorMessages = "";
				foreach (ValidationError error in result.Errors.DeepAll())
				{
					errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
				}
				return Json(new { isSuccess = false, message = errorMessages });
			}
		}

		[HttpPost]
		[Route("thanh-toan")]
		public async Task<IActionResult> PlaceOrder(InputOrderModel inputModel)
		{
			var baseUrl = $"http://localhost:5260/api/Orders/them-don-hang";
			var content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, content);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				var message = JsonConvert.DeserializeObject<dynamic>(result)!.message;
				return RedirectToAction("HistoryOrder", "Carts", new { area = "", message = message.ToString() });
			}
			return RedirectToAction("Index", "Carts", new { area = "" });
		}

		[HttpPost]
		[Route("giam-gia")]
		public async Task<IActionResult> GetDiscountCode(string promotionName, long toTal)
		{
			var baseUrl = $"http://localhost:5260/api/Promotions/discount?promotionName={promotionName}";
			const int max = 100000;
			var discountResponse = await _httpClient.GetAsync(baseUrl);
			if (discountResponse.IsSuccessStatusCode)
			{
				var discountJson = await discountResponse.Content.ReadAsStringAsync();
				var discount = JsonConvert.DeserializeObject<int>(discountJson);
				var discountCode = toTal * discount / 100;
				return Json(discountCode < max ? discountCode : max);
			}
			return Json(0);
		}

		[Route("lich-su-don-hang")]
		public async Task<IActionResult> HistoryOrder(string message)
		{
			var orderResponse = new OrderResponse();
			var baseUrl = $"http://localhost:5260/api/Orders/danh-sach-don-hang";
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var orders = JsonConvert.DeserializeObject<ListViewModel<OrderEx>>(json);
				if (orders != null)
				{
					orderResponse.Orders = orders;
					orderResponse.Message = message;
					orderResponse.Success = true;
				}
				else
				{
					orderResponse.Success = false;
					orderResponse.Message = "Không thể chuyển đổi dữ liệu JSON thành danh sách đơn hàng.";
				}
			}
			else
			{
				orderResponse.Success = false;
				orderResponse.Message = "Không thể lấy danh sách đơn hàng từ API.";
			}
			return View(orderResponse);
		}

		[Route("chi-tiet-don-hang")]
		public async Task<IActionResult> GetOrderItems(string orderName)
		{
			var baseUrl = $"http://localhost:5260/api/OrderItems/chi-tiet-don-hang?orderName={orderName}";
			var orderItemsViewModel = new OrderItemsViewModel();
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				orderItemsViewModel = JsonConvert.DeserializeObject<OrderItemsViewModel>(json);
			}
			return PartialView("_OrderItemList", orderItemsViewModel);
		}

		[Route("thong-tin-don-hang")]
		public async Task<IActionResult> GetOrderInfo(string orderName)
		{
			var baseUrl = $"http://localhost:5260/api/Orders/don-hang?orderName={orderName}";
			var order = new OrderEx();
			var response = await _httpClient.GetAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				order = JsonConvert.DeserializeObject<OrderEx>(json);
			}
			return Json(new
			{
				paymentName = order?.PaymentName,
				textColorPaymentName = order?.TextColorPaymentName,
				backgroundColorPaymentName = order?.BackgroundColorPaymentName,
				promotionName = order?.PromotionName,
				orderStatusName = order?.OrderStatusName,
				textColorOrderStatusName = order?.TextColorOrderStatusName,
				backgroundColorOrderStatusName = order?.BackgroundColorOrderStatusName,
				ghiChu = order?.GhiChu,
				discountCode = order?.DiscountCode,
				discountValue = order?.DiscountValue,
				orderDatetime = order?.OrderDatetime,
				deliveryDate = order?.DeliveryDate,
				deliveryTimeSlot = order?.DeliveryTimeSlot
			});
		}

	}
}
