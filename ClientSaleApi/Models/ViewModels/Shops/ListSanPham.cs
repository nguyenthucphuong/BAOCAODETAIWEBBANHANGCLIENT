namespace ClientSaleApi.Models.ViewModels.Shops
{
    public class ListSanPham
    {
        public int TotalCount { get; set; }
		public List<OutputShop> Items { get; set; } = new List<OutputShop>();
		public int AllPriceCount { get; set; }
        public int Price1Count { get; set; }
        public int Price2Count { get; set; }
        public int Price3Count { get; set; }
    }
}
