using ClientSaleApi.Models.ViewModels.Shops;
using ClientSaleApi.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using X.PagedList;


namespace ClientSaleApi.Models.Reports
{
	public class RevenueAndRefundItem
	{
		public DateTime OrderDatetime { get; set; }
		public long Revenue { get; set; }
		public long Refund { get; set; }
	}

	public class RevenueViewModels
	{
		public List<RevenueAndRefundItem> Items { get; set; } = new List<RevenueAndRefundItem>();
		public int TotalOrders { get; set; }
		public long TotalRevenue { get; set; }
		public long TotalRefund { get; set; }
		public string ReportType { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalCount { get; set; }
	}

	public class ReportIndexViewModel : IndexViewModel<RevenueAndRefundItem>
	{
		public int TotalOrders { get; set; }
		public long TotalRevenue { get; set; }
		public long TotalRefund { get; set; }
		public string ReportType { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}

}