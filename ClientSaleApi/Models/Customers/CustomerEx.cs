using ClientSaleApi.Models.Payments;

namespace ClientSaleApi.Models.Customers
{
    public class CustomerEx
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Ward { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public CustomerEx() { }
        

    }
}
