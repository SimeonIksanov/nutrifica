using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using MudBlazor;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Shared;

public partial class LoginComponent : IDisposable
{
    [Inject] private NutrificaAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }
    private TokenRequest Model { get; set; } = null!;
    private EditContext _editContext = null!;
    private ValidationMessageStore _messageStore = null!;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    protected override void OnInitialized()
    {
        Model = new();
        _editContext = new EditContext(Model);
        _editContext.OnValidationRequested += HandleValidationRequested;
        _messageStore = new(_editContext);
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        _messageStore.Clear();
    }

    public async Task HandleFormSubmit()
    {
        IResult<User> result;
        try
        {
            result = await AuthenticationStateProvider.LoginAsync(Model, _cancellationTokenSource!.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = Result.Failure<User>(new Error("Exception", ex.Message));
        }

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error.Description, Severity.Error, options => options.CloseAfterNavigation = true);
            Model.Password = string.Empty;
            return;
        }

        NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(ReturnUrl) ? "/" : ReturnUrl);
    }

    public void Dispose()
    {
        if (_editContext != null)
            _editContext.OnValidationRequested -= HandleValidationRequested;
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        AuthenticationStateProvider.Dispose();
        Snackbar.Dispose();
    }
}
