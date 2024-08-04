using Nutrifica.Application.Models.Users;

namespace Nutrifica.Application.Models.Clients;

public record PhoneCallModel(
    int Id,
    DateTime CreatedOn,
    UserFullName CreatedBy,
    // ICollection<ProductModel> products
    string Comment);