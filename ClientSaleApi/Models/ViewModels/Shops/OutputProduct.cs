namespace ClientSaleApi.Models.ViewModels.Shops
{
    public class OutputProduct: ApiResponse
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
		public string FriendlyUrl { get; set; } = string.Empty;
		public int? ProductPrice { get; set; }
        public string? ProductDes { get; set; }
        public string? ProductImage { get; set; }
        public int? DiscountPrice { get; set; }
    }
}
