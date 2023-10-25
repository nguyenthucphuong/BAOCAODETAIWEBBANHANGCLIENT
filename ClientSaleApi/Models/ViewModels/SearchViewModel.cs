using System.ComponentModel.DataAnnotations;

namespace ClientSaleApi.Models.ViewModels
{
        public class SearchViewModel
        {

            [Range(0, int.MaxValue)]
            public int SkipCount { get; set; } = 0;
            [Range(0, int.MaxValue)]
            public int MaxResultCount { get; set; } = 6;
            public string? Search { get; set; }
		    public string? Sorting { get; set; }
	    }
}
