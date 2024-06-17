using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Pages;

public partial class Login : IDisposable //: ComponentBase
{
    [Inject] private IAuthenticationService _authenticationService { get; set; }
    private TokenRequest _model { get; set; }
    private EditContext? _editContext;
    private ValidationMessageStore? _messageStore;
    private CancellationTokenSource? _cancellationTokenSource;

    protected override void OnInitialized()
    {
        _model = new();
        _editContext = new EditContext(_model);
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
        var authResult = await _authenticationService.LoginAsync(_model, _cancellationTokenSource!.Token);
        if (authResult.IsFailure)
        {
            _messageStore?.Add(() => _model, authResult.Error.Description);
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

public class LoginForm
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Login не может быть пустым!")]
    public string Login { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password не может быть пустым!")]
    public string Password { get; set; } = string.Empty;
}
