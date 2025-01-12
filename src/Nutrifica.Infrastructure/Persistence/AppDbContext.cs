using System.Reflection;

using MediatR;

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
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMediator _mediator;

    public AppDbContext(DbContextOptions options,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IMediator mediator) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _mediator = mediator;
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
    public DbSet<UserClientAccess> UserClientAccesses { get; set; }
    public DbSet<UserOrderAccess> UserOrderAccess { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder
            .HasDbFunction(() => GetEmployeeTree(default))
            .HasName("GetEmployeeTree");
    }

    public IQueryable<UserShortModel> GetEmployeeTree(Guid userId)
        => FromExpression(() => GetEmployeeTree(userId));

    public override int SaveChanges()
    {
         return Task.Run(async () => await SaveChangesAsync()).Result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        OnAfterSaveChanges();
        return result;
    }

    private async Task OnBeforeSaveChanges()
    {
        ChangeModifiedProperties();

        await PublishDomainEventsAsync();
    }

    private void ChangeModifiedProperties()
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

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }

    private void OnAfterSaveChanges()
    {
        // throw new NotImplementedException();
    }
}