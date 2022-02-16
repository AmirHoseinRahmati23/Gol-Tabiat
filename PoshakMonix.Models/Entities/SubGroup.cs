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
    public class SubGroup : Entity
    {
        [Display(Name = "نام زیر گروه")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Name { get; set; }


        //Navigation Properties
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
