namespace ClientSaleApi.Models.ViewModels
{
    public class SearchPartialModel
    {
        public string? ControllerName { get; set; }
        SearchPartialModel() { }
        public SearchPartialModel(string name)
        {
            ControllerName = name;
        }
    }
}
