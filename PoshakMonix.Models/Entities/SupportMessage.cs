using PoshakMonix.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class SupportMessage : Entity
    {
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string UserName { get; set; }


        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [EmailAddress]
        public string Email { get; set; }


        [MaxLength(800, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Text { get; set; }

    }
}
