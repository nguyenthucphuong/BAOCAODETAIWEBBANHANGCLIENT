namespace ClientSaleApi.Models.ViewModels
{
    public class ListViewModel<T>: SimpleListViewModel<T>
    {
        public int TotalCount { get; set; }

    }

}

