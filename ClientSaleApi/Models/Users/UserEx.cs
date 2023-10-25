using ClientSaleApi.Models.Accounts;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;

namespace ClientSaleApi.Models.Users
{
    public class UserEx
    {
		public string? UserId { get; set; } = string.Empty;
		public string RoleId { get; set; } = string.Empty;
		public string RoleName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
		public bool IsActive { get; set; }
        public UserEx() { }
        public UserEx(InputUser input)
        {
            UserName = input.UserName;
            Email = input.Email;
            Password = input.Password;
			RoleName = input.RoleName;
            IsActive = input.IsActive == "on" ? true : false;
        }
        
    }
}
