using ClientSaleApi.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Accounts
{
    public class InputChangePassword : ApiResponse
	{
        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!])[A-Za-z\d@!]{6,}$", ErrorMessage = "Mật khẩu phải bao gồm ít nhất 6 ký tự, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt (@, !,...)")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
