using ClientSaleApi.Models.Customers;
using ClientSaleApi.Models.OrderItems;

namespace ClientSaleApi.Models.Orders
{
    public class InputOrderModel
    {
        public InputOrder InputOrder { get; set; } = new InputOrder();
        public List<InputOrderItem> InputOrderItems { get; set; } = new List<InputOrderItem>();
        public InputCustomer InputCustomer { get; set; } = new InputCustomer();
    }
}
