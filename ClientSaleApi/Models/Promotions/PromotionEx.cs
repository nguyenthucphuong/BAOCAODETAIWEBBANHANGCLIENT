using ClientSaleApi.Models.Payments;
using ClientSaleApi.Models.Users;
namespace ClientSaleApi.Models.Promotions
{
    public class PromotionEx
    {
        public string PromotionId { get; set; } = string.Empty;
        public string PromotionName { get; set; } = string.Empty;
        public string? PromotionDes { get; set; }
        public string? UserName { get; set; }
        public int? Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public PromotionEx() { }
        public PromotionEx(InputPromotion input)
        {
            PromotionName = input.PromotionName;
            PromotionDes = input.PromotionDes;
            Discount = input.Discount;
            StartDate = input.StartDate;
            EndDate = input.EndDate;
            IsActive = input.IsActive == "on" ? true : false;
        }
    }
}
