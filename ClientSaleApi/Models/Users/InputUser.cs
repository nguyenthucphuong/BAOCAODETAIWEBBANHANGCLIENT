using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Users
{
    public class InputUser
    {
        public string? UserId { get; set; }
		[Required(ErrorMessage = "Bạn phải chọn vai trò")]
		public string RoleName { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^[a-z0-9]*$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ thường và số")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Tên đăng nhập phải có độ dài từ 1 đến 50 ký tự")]
		public string UserName { get; set; } = string.Empty;
		[Required]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; } = string.Empty;
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!])[A-Za-z\d@!]{6,}$", ErrorMessage = "Mật khẩu phải bao gồm ít nhất 6 ký tự, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt (@, !,...)")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Mật khẩu phải có độ dài từ 1 đến 50 ký tự")]
		public string Password { get; set; } = string.Empty;
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		public string ConfirmPassword { get; set; } = string.Empty;
		public string IsActive { get; set; } = string.Empty;
    }
}
