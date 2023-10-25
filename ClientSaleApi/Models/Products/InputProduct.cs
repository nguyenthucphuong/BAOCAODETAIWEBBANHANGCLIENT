using System.ComponentModel.DataAnnotations;
namespace ClientSaleApi.Models.Products
{
    public class InputProduct
    {
        public string? ProductId { get; set; }
		public string? CategoryName { get; set; }
        public string? PromotionName { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string ProductName { get; set; } = string.Empty;
        public string ProductPrice { get; set; } = string.Empty;
        public string? ProductDes { get; set; }
        public IFormFile? imageFile { get; set; }
        public string? ProductImage { get; set; }
        public string IsNew { get; set; } = string.Empty;
        public string IsSale { get; set; } = string.Empty;
        public string IsPro { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
    }
}
