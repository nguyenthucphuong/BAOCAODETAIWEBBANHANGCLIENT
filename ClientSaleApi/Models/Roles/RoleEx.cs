namespace ClientSaleApi.Models.Roles
{
    public class RoleEx
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public RoleEx() { }
        public RoleEx(InputRole input)
        {
            RoleName = input.RoleName;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
