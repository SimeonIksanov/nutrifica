using Microsoft.EntityFrameworkCore;
using Nutrifica.Domain.ClientAggregate;
using Nutrifica.Domain.OrderAggregate;
using Nutrifica.Domain.ProductAggregate;
using Nutrifica.Domain.UserAggregate;
using Nutrifica.Domain.UserAggregate.Entities;
using Nutrifica.Domain.UserAggregate.ValueObjects;

namespace Nutrifica.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        // Database.EnsureCreated();
    }

    // public DbSet<User> Users { get; set; }
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
        //
        // modelBuilder.Entity<User>()
        //     .HasKey(x => x.Id);
        // modelBuilder.Entity<User>()
        //     .Property(x => x.Id)
        //     .HasConversion<int>(x => x.Value, x => UserId.Create(x));
        // modelBuilder.Entity<User>()
        //     .HasOne<UserAccount>(x => x.Account)
        //     .WithOne(x => x.User)
        //     .HasForeignKey<UserAccount>("userId");
    }
}