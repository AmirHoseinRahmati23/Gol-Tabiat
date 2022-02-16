using PoshakMonix.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class Group : Entity
    {
        [Display(Name = "نام گروه")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(1, ErrorMessage = "لطفا یچیزی وارد کنید")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string GroupName { get; set; }


        // Navigation Properties
        public ICollection<SubGroup> SubGroups { get; set; }
    }
}
