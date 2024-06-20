using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Infrastructure.Persistence.Configurations;

namespace Nutrifica.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }

    // public DbSet<Client> Clients { get; set; }
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // modelBuilder.ApplyConfiguration(new UserConfiguration());
        // modelBuilder.ApplyConfiguration(new ClientConfiguration());
        // modelBuilder.ApplyConfiguration(new OrderConfiguration()); // <-- here
    }
}