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

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<ItemAssignment> ItemAssignments { get; set; }

        public DbSet<Request> Requests { get; set; }
    }
}
