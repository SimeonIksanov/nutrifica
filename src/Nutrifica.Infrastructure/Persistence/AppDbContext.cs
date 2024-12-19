using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.NotificationAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AppDbContext(DbContextOptions options, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        // Database.EnsureDeleted();
        Database.EnsureCreated();
        // TODO: Configure Indexes
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<PhoneCall> PhoneCalls { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder
            .HasDbFunction(()=>GetEmployeeTree(default))
            .HasName("GetEmployeeTree");
        var builder = modelBuilder.Entity<UserShortModel>();
        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));
        builder.Property(x=>x.FirstName)
            .HasConversion<string>(
                x => x.Value,
                x => FirstName.Create(x));
        builder.Property(x=>x.MiddleName)
            .HasConversion<string>(
                x => x.Value,
                x => MiddleName.Create(x));
        builder.Property(x=>x.LastName)
            .HasConversion<string>(
                x => x.Value,
                x => LastName.Create(x));
    }

    public IQueryable<UserShortModel> GetEmployeeTree(Guid userId)
        => FromExpression(()=>GetEmployeeTree(userId));

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        OnAfterSaveChanges();
        return result;
    }

    private void OnBeforeSaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.CreatedOn = _dateTimeProvider.UtcNow;
                    entry.Entity.LastModifiedOn = _dateTimeProvider.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedOn = _dateTimeProvider.UtcNow;
                    break;
            }
        }
    }

    private void OnAfterSaveChanges()
    {
        // throw new NotImplementedException();
    }
}