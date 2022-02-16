using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<SliderImage> SliderImages { get; set; }
        public List<Product> NewestProducts { get; set; }
        public List<Product> SecoundCardSliderItems { get; set; }
    }
}
