using ClientSaleApi.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Accounts
{
	public class InputCheckEmail : ApiResponse
	{
        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
	}
}
