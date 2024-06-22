using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Pages;

public partial class Login : IDisposable //: ComponentBase
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NutrificaAuthenticationStateProvider _authenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private LoginModel Model { get; set; } = null!;
    private EditContext? _editContext;
    private ValidationMessageStore? _messageStore;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool ShowAuthError { get; set; }
    private string AuthError { get; set; }

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
        ShowAuthError = false;
        var request = new TokenRequest(Model.Username, Model.Password);
        var result = await _authenticationStateProvider.LoginAsync(request, _cancellationTokenSource!.Token);
        if (result.IsFailure)
        {
            AuthError = result.Error.Description;
            ShowAuthError = true;
            return;
        }
        NavigationManager.NavigateTo("/");
    }

    public void Dispose()
    {
        if (_editContext != null)
            _editContext.OnValidationRequested -= HandleValidationRequested;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _authenticationStateProvider?.Dispose();
    }
}

public class LoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя пользователя не может быть пустым!")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым!")]
    public string Password { get; set; } = string.Empty;
}