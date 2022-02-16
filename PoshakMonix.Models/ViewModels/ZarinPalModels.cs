using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.ViewModels
{
    public class ZarinPalRequestResponseModel
    {
        public int Status { get; set; }
        public string Authority { get; set; }
    }
    public class ZarinPalVerifyResponseModel
    {
        public int Status { get; set; }
        public string RefID { get; set; }
    }
}
