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
    public class CartDetail : Entity
    {
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public int Price { get; set; }




        //Foreign Keys
        public int CartId { get; set; }

        public int ProductId { get; set; }

        //Navigation Properties
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        [ForeignKey( nameof(ProductId) )]
        public Product Product { get; set; }

    }
}
