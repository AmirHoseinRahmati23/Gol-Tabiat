using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Core.Utilities
{
    public static class Generator
    {
        public static string GenerateUniqueName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
