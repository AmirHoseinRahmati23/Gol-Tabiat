using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Entities
{
    public class User : IdentityUser
    {











        //Navigation Properties
        public ICollection<Cart> Cart { get; set; }
    }
}
