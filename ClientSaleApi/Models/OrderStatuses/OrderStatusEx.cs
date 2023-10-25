namespace ClientSaleApi.Models.OrderStatuses
{
    public class OrderStatusEx
    {
        public string OrderStatusId { get; set; } = string.Empty;
        public string OrderStatusName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public OrderStatusEx() { }
        public OrderStatusEx(InputOrderStatus input)
        {
            OrderStatusName = input.OrderStatusName;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
