using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Roles
{
    public class InputRole
    {
        public string? RoleId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Tên chỉ gồm: chữ cái và số")]
		public string RoleName { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;

    }
}
