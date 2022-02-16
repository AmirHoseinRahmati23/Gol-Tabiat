using Microsoft.EntityFrameworkCore;
using PoshakMonix.Core.UnitOfWork.Repositories;
using PoshakMonix.Data;
using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshakMonix.Core.UnitOfWork.Services
{
    public class GroupService : IGroupRepository
    {
        private readonly PoshakMonixContext _context;
        public GroupService(PoshakMonixContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            var groups = await _context.Groups.Include(g => g.SubGroups).ToListAsync();

            return groups;
        }
    }
}
