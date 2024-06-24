using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using MudBlazor;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Shared;

public partial class LoginComponent : IDisposable //: ComponentBase
{
    [Inject] private NutrificaAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }
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
    }

    public async Task HandleFormSubmit()
    {
        IResult<User> result;
        try
        {
            var request = new TokenRequest(Model.Username, Model.Password);
            result = await AuthenticationStateProvider.LoginAsync(request, _cancellationTokenSource!.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = Result.Failure<User>(new Error("Exception", ex.Message));
        }

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error.Description, Severity.Error, options => options.CloseAfterNavigation = true);
            return;
        }

        NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(ReturnUrl) ? "/" : ReturnUrl);
    }

    public void Dispose()
    {
        if (_editContext != null)
            _editContext.OnValidationRequested -= HandleValidationRequested;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        AuthenticationStateProvider.Dispose();
        Snackbar.Dispose();
    }
}

public class LoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя пользователя не может быть пустым!")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым!")]
    public string Password { get; set; } = string.Empty;
}