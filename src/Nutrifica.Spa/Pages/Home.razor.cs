using Microsoft.AspNetCore.Components;

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
        base.OnInitialized();
    }


    private void OnClick()
    {
        Counter_++;
    }
}
