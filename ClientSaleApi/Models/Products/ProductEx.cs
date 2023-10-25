using System.ComponentModel.DataAnnotations;
namespace ClientSaleApi.Models.Products
{
    public class ProductEx
    {
        public string ProductId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? CategoryName { get; set; }
        public string? PromotionName { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string FriendlyUrl { get; set; } = string.Empty;
        public int? ProductPrice { get; set; }
        public int? DiscountPrice { get; set; }
        public string? ProductDes { get; set; }
        public string? ProductImage { get; set; }
        public IFormFile? imageFile { get; set; }
		public bool IsNew { get; set; }
        public bool IsSale { get; set; }
        public bool IsPro { get; set; }
        public bool IsActive { get; set; }
        public ProductEx() { }
        public ProductEx(InputProduct input)
        {
            CategoryName = input.CategoryName ?? "";
            PromotionName = input.IsPro != "on" ? "" : (input.PromotionName ?? "");
            ProductName = input.ProductName ?? "";
            ProductImage = input.ProductImage ?? "";
            ProductPrice = string.IsNullOrEmpty(input.ProductPrice) ? 0 : int.Parse(input.ProductPrice.Replace(",", ""));
            ProductDes = input.ProductDes ?? "";
            this.imageFile = input.imageFile;
            IsNew = input.IsNew == "on" ? true : false;
            IsSale = input.IsSale == "on" ? true : false;
            IsPro = input.IsPro == "on" ? true : false;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
