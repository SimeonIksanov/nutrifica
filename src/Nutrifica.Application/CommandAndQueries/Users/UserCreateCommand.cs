using FluentValidation;
using Nutrifica.Application.Interfaces;
using Nutrifica.Domain.UserAggregate;

namespace Nutrifica.Application.CommandAndQueries.Users;

public class UserCreateCommand : ICommand<User>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
}

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(30);
    }
}