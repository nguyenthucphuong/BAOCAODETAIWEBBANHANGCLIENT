using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.Categories
{
    public class CategoryEx
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public CategoryEx() { }
        public CategoryEx(InputCategory input)
        {
            CategoryName = input.CategoryName;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
