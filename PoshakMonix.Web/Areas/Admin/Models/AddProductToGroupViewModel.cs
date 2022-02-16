using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoshakMonix.Web.Areas.Admin.Models
{
    public class AddProductToGroupViewModel
    {
        public Product Product { get; set; }
        public List<SubGroup> Groups { get; set; }
    }
}
