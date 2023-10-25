using ClientSaleApi.Areas.Admin.Views.Reports;
using ClientSaleApi.Models.Payments;
using ClientSaleApi.Models.Reports;
using ClientSaleApi.Models.ViewModels;
using ClientSaleApi.Models.ViewModels.Shops;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Composition;
using System.Text;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientSaleApi.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
    [Route("bao-cao")]
    public class ReportsController : BaseController<ReportEx, ListViewModel<ReportEx>, object>
    {

        private readonly HttpClient _httpClient;

        public ReportsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        [Route("don-hang")]
        public async Task<IActionResult> Index(SearchViewModel searchModel, int? page, string? message)
        {
            var baseUrl = "http://localhost:5260/api/Reports/bao-cao-danh-sach-don-hang";
            var result = await base.Index(baseUrl, searchModel, page, message);
            return result;
        }

        [Route("cap-nhat-trang-thai-don-hang")]
        [HttpPut]
        public async Task<IActionResult> CapNhatTrangThaiOrder([FromBody] OrderStatusUpdateModel model)
        {
            var response = await _httpClient.PutAsJsonAsync("http://localhost:5260/api/Orders/cap-nhat-trang-thai-don-hang", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

		[Route("doanh-thu")]
		public async Task<IActionResult> Revenue([FromQuery] InputRevenueReport input)
		{
			var baseUrl = "http://localhost:5260/api/Reports/doanh-thu";
			var url = $"{baseUrl}?reportType={input.ReportType}&startDate={input.StartDate:yyyy-MM-dd}&endDate={input.EndDate:yyyy-MM-dd}&page={input.Page}&pageSize={input.PageSize}";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<RevenueViewModels>();
				var items = new StaticPagedList<RevenueAndRefundItem>(result.Items, input.Page, input.PageSize, result.TotalCount);
				var viewModel = new ReportIndexViewModel
				{
					Items = items,
					PagingModel = new PagingViewModel
					{
						Page = input.Page,
						PageSize = input.PageSize,
						TotalCount = result.TotalCount,
					},
					TotalOrders = result.TotalOrders,
					TotalRevenue = result.TotalRevenue,
					TotalRefund = result.TotalRefund,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
					ReportType = input.ReportType
				};
				return View(viewModel);
			}
			return View(new ReportIndexViewModel());
		}

		[HttpGet]
        [Route("export")]
        public async Task<IActionResult> ExportToExcel(SearchViewModel searchModel, int page = 1)
        {
            var baseUrl = "http://localhost:5260/api/Reports/bao-cao-danh-sach-don-hang";
            var list = await GetDanhSachModelAsync(baseUrl, searchModel);
            var items = new StaticPagedList<ReportEx>(list.Items, searchModel.MaxResultCount, page, list.TotalCount);
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    worksheet.Cell(1, 1).Value = "STT";
                    worksheet.Cell(1, 2).Value = "OrderName";
                    worksheet.Cell(1, 3).Value = "User";
                    worksheet.Cell(1, 4).Value = "Customer";
                    worksheet.Cell(1, 5).Value = "Payment";
                    worksheet.Cell(1, 6).Value = "OrderDatetime";
                    worksheet.Cell(1, 7).Value = "DeliveryDate";
                    worksheet.Cell(1, 8).Value = "OrderStatus";
                    worksheet.Cell(1, 9).Value = "Total";
                    int rowIndex = 2;
                    foreach (var item in items)
                    {
                        worksheet.Cell(rowIndex, 1).Value = rowIndex - 1;
                        worksheet.Cell(rowIndex, 2).Value = item.OrderName;
                        worksheet.Cell(rowIndex, 2).Value = item.UserName;
                        worksheet.Cell(rowIndex, 4).Value = item.CustomerName + "\n" + item.PhoneNumber;
                        worksheet.Cell(rowIndex, 4).Style.Alignment.WrapText = true;
                        worksheet.Cell(rowIndex, 5).Value = item.PaymentName;
                        worksheet.Cell(rowIndex, 6).Value = item.OrderDatetime.ToString("dd/MM/yyyy HH:mm:ss");
                        worksheet.Cell(rowIndex, 7).Value = item.DeliveryDate.ToString("dd/MM/yyyy") + "\n" + item.DeliveryTimeSlot;
                        worksheet.Cell(rowIndex, 7).Style.Alignment.WrapText = true;
                        worksheet.Cell(rowIndex, 8).Value = item.OrderStatusName;
                        if (item.Refund > 0)
                        {
                            worksheet.Cell(rowIndex, 9).Value = "-" + item.Total.ToString("N0");
                        }
                        else
                        {
                            worksheet.Cell(rowIndex, 9).Value = item.Total.ToString("N0");
                        }
                        rowIndex++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
						return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString() + ".xlsx");
                    }
                }
            }
            catch (Exception)
            {
                return Ok();
            }

        }
    }
}
