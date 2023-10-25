namespace ClientSaleApi.Models.ViewModels
{
    public class SimpleListViewModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}
