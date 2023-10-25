﻿namespace ClientSaleApi.Models.Reports
{
	public class InputRevenueReport
	{
		public string ReportType { get; set; } = "date";
		public DateTime StartDate { get; set; } = DateTime.Today;
		public DateTime EndDate { get; set; } = DateTime.Today;
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 6;
	}
}
