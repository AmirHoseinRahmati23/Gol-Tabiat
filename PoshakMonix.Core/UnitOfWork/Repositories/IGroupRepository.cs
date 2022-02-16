using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoshakMonix.Models.Entities;

namespace PoshakMonix.Core.UnitOfWork.Repositories
{
    public interface IGroupRepository
    {
        public Task<List<Group>> GetAllGroups();
    }
}
