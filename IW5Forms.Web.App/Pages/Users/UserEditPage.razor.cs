using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.User;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class UserEditPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private UserFacade UserFacade { get; set; } = null!;

    private UserDetailModel Data { get; set; } = GetNewUser();

    [Parameter]
    public Guid Id { get; init; }

    private bool isValid;

    private bool IsAdmin
    {
        get => Data.Role == Common.Enums.RoleTypes.Admin ? true : false;
        set
        {
            if (value)
                Data.Role = Common.Enums.RoleTypes.Admin;
            else
                Data.Role = Common.Enums.RoleTypes.User;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            Data = await UserFacade.GetByIdAsync(Id);
        }

        await base.OnInitializedAsync();
    }

    public async Task Save()
    {
        await UserFacade.SaveAsync(Data);
        navigationManager.NavigateTo($"/users");
    }

    private static UserDetailModel GetNewUser() => new() {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        PhotoUrl = string.Empty,
        Role = Common.Enums.RoleTypes.User,
        Forms = new List<UserDetailFormModel>()
    };
}
