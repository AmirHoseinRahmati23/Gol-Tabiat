using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.ViewModels
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            Products = new PaginationViewModel<Product>();
            Groups = new List<SubGroup>();
        }
        public PaginationViewModel<Product> Products { get; set; }
        public List<SubGroup> Groups { get; set; }
        public string SearchText { get; set; }
        
    }
}
