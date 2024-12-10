using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.User;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class UserDetailPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private UserFacade UserFacade { get; set; } = null!;

    private UserDetailModel Data { get; set; } = null!;

    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        Data = await UserFacade.GetByIdAsync(Id);

        await base.OnInitializedAsync();
    }
}
