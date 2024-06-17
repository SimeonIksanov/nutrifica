using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace Nutrifica.Spa.Pages;

partial class Home
{
    [Parameter]
    public int? Counter { get; set; }
    public int Counter_ { get; set; }

    public void NotifyChange()
    {
        InvokeAsync(StateHasChanged);
    }
    protected override void OnInitialized()
    {
        Counter_ = Counter ?? 10;
        navManager.LocationChanged += OnLocationChanged;
        base.OnInitialized();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Console.WriteLine(e.Location);
    }

    private void OnClick()
    {
        Counter_++;
    }
    public void Dispose()
    {
        navManager.LocationChanged -= OnLocationChanged;
    }
}
