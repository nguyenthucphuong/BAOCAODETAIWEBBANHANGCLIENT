using System.ComponentModel.DataAnnotations;
namespace ClientSaleApi.Models.OrderStatuses
{
    public class InputOrderStatus
    {
        public string? OrderStatusId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string OrderStatusName { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
    }
}
