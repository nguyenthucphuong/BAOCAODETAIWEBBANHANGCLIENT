namespace ClientSaleApi.Models.OrderItems
{
    public class OrderItemEx
    {
        public string OrderItemName { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int? Discount { get; set; }
        public DateTime OrderItemDatetime { get; set; }
        public string ProductId { get; set; } = string.Empty;

    }
}
