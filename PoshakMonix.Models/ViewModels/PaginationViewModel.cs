using System.Collections.Generic;

namespace PoshakMonix.Models.ViewModels
{
    public class PaginationViewModel<T>
    {
        public PaginationViewModel()
        {
            Items = new List<T>();
        }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int ItemsInPage { get; set; }
        public List<T> Items { get; set; }
    }
}