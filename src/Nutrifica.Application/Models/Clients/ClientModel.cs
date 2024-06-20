using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Models.Clients;

public record ClientModel(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    Address Address,
    string Comment,
    string PhoneNumber,
    string Source,
    DateTime CreatedAt);

public record ClientDetailedModel(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    Address Address,
    string Comment,
    string PhoneNumber,
    string Source,
    DateTime CreatedAt,
    ICollection<PhoneCallModel> phoneCalls);

public record PhoneCallModel(
    int Id,
    DateTime CreatedAt,
    Guid CreatedById,
    string CreatedByName,
    // ICollection<ProductModel> products
    string Comment);

