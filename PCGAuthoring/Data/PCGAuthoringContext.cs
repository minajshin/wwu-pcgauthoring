using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCGAuthoring.Models;

namespace PCGAuthoring.Data
{
    public class PCGAuthoringContext : DbContext
    {
        public PCGAuthoringContext (DbContextOptions<PCGAuthoringContext> options)
            : base(options)
        {
        }

        public DbSet<PCGAuthoring.Models.Room> Rooms { get; set; }

        public DbSet<PCGAuthoring.Models.Item> Items { get; set; }

        public DbSet<PCGAuthoring.Models.ItemAssignment> ItemAssignments { get; set; }
    }
}
