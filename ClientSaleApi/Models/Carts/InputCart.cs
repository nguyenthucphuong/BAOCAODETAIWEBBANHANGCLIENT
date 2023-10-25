namespace ClientSaleApi.Models.Carts
{
    public class InputCart
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
		public string FriendlyUrl { get; set; } = string.Empty;
	}
}
