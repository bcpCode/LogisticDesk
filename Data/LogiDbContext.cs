namespace LogisticDesk.Data;
using LogisticDesk.Models;
using Microsoft.EntityFrameworkCore;

public class LogiDbContext : DbContext
{
    public LogiDbContext(DbContextOptions<LogiDbContext> options) : base(options)
    {
    }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
}
