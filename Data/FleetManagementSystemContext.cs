using Microsoft.EntityFrameworkCore;
using FleetManagementSystem.Models;

namespace FleetManagementSystem.Data
{
    public class FleetManagementSystemContext : DbContext
    {
        public FleetManagementSystemContext (DbContextOptions<FleetManagementSystemContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; } = default!;
    }
}
