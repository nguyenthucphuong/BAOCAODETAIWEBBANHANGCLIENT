using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Promotions
{
	public class InputPromotion
    {
        public string? PromotionId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string PromotionName { get; set; } = string.Empty;
        public string? PromotionDes { get; set; }
		[Range(0, 100, ErrorMessage = "Vui lòng nhập giá trị Khuyến mãi 0 - 100!")]
		public int? Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IsActive { get; set; } = string.Empty;
    }
}
