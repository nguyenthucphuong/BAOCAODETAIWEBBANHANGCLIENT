using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Payments
{
    public class InputPayment
    {
        public string? PaymentId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string PaymentName { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;

    }
}
