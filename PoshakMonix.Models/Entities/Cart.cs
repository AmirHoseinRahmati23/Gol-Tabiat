using PoshakMonix.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class Cart : Entity
    {

        public int FinallPrice { get; set; }
        public bool IsFinaly { get; set; }

        //Navigation Properties
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
