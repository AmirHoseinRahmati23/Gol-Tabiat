using PoshakMonix.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class Factor : Entity
    {
        [Display(Name = "ارسال شده")]
        public bool Sended { get; set; }
        [Display(Name = "پرداخت شده")]
        public bool IsPayed { get; set; }

        [Display(Name ="آدرس")]
        [Required]
        public string Address { get; set; }

        public int CartId { get; set; }

        //Navigation Properties

        public ICollection<FactorDetail> FactorDetails { get; set; }
        [ForeignKey(nameof(UserId))]
        public ICollection<User> User { get; set; }
        public string UserId { get; set; }
    }
}
