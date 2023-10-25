namespace ClientSaleApi.Models.Orders
{
    public class OrderEx
    {
        public string OrderName { get; set; } = string.Empty;
        public string PaymentName { get; set; } = string.Empty;
        public string TextColorPaymentName { get; set; } = string.Empty;
        public string BackgroundColorPaymentName { get; set; } = string.Empty;
        public string? PromotionName { get; set; } = string.Empty;
        public int? DiscountCode { get; set; }
        public string OrderStatusName { get; set; } = string.Empty;
        public string TextColorOrderStatusName { get; set; } = string.Empty;
        public string BackgroundColorOrderStatusName { get; set; } = string.Empty;
        public long Total { get; set; }
        public DateTime OrderDatetime { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? DeliveryTimeSlot { get; set; }
        public string? GhiChu { get; set; }
		public int? DiscountValue { get; set; }
	}
}
