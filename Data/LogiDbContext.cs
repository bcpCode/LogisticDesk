namespace LogisticDesk.Data;
using LogisticDesk.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LogiDbContext : IdentityDbContext<ApplicationUser>
{
    public LogiDbContext(DbContextOptions<LogiDbContext> options) : base(options)
    {
    }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
}
