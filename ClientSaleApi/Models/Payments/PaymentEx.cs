namespace ClientSaleApi.Models.Payments
{
    public class PaymentEx
    {
        public string PaymentId { get; set; } = string.Empty;
        public string PaymentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public PaymentEx() { }
        public PaymentEx(InputPayment input)
        {
            PaymentName = input.PaymentName;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
