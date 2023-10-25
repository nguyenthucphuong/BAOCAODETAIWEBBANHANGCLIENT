using System.ComponentModel.DataAnnotations;
namespace ClientSaleApi.Models.Categories
{
    public class InputCategory
    {
        public string? CategoryId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string CategoryName { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
    }
}
