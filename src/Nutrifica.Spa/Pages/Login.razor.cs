using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Pages;

public partial class Login : IDisposable //: ComponentBase
{
    [Inject] private IAuthenticationService _authenticationService { get; set; }
    private LoginModel Model { get; set; } = null!;
    private EditContext? _editContext;
    private ValidationMessageStore? _messageStore;
    private CancellationTokenSource? _cancellationTokenSource;

    protected override void OnInitialized()
    {
        Model = new();
        _editContext = new EditContext(Model);
        _editContext.OnValidationRequested += HandleValidationRequested;
        _messageStore = new(_editContext);
        _cancellationTokenSource = new();
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        _messageStore?.Clear();
        // _messageStore?.Add(()=>Model.Login, "тестовое сообщение об ошибке");
        // _messageStore?.Add(()=>Model, "тестовое сообщение об ошибке");
    }

    public async Task HandleFormSubmit()
    {
        var request = new TokenRequest(Model.Username, Model.Password);
        var authResult = await _authenticationService.LoginAsync(request, _cancellationTokenSource!.Token);
        if (authResult.IsFailure)
        {
            _messageStore?.Add(() => Model, authResult.Error.Description);
        }
    }

    public void Dispose()
    {
        if (_editContext != null)
            _editContext.OnValidationRequested -= HandleValidationRequested;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}

public class LoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username не может быть пустым!")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password не может быть пустым!")]
    public string Password { get; set; } = string.Empty;
}