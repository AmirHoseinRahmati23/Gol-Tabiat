using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Models.Base
{
    public abstract class Entity : object
    {
        [Key]
        public int Id { get; set; }
    }
}
