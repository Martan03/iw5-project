using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.User;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

[Authorize]
public partial class UserListPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private UserFacade UserFacade { get; set; } = null!;

    private ICollection<UserListModel> Users { get; set; } =
        new List<UserListModel>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Users = await UserFacade.GetAllAsync();
        }
        catch (Exception)
        {

            throw;
        }

        await base.OnInitializedAsync();
    }

    public Task DeleteDialog(Guid id)
    {
        var parameters = new DialogParameters<Dialog>
        {
            { x => x.ContentText, "Do you really want to delete this user?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error },
            { x => x.OnSubmit, EventCallback.Factory.Create(
                this, async () => await Delete(id)
            )},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

        return DialogService.ShowAsync<Dialog>("Delete", parameters, options);
    }

    public async Task DeleteAsync(Guid id)
    {
       await UserFacade.DeleteAsync(id);
       var user =  Users.FirstOrDefault(user => user.Id == id);
        if (user != null)
            Users.Remove(user);
    }

    public void NavDetail(Guid id)
    {
        navigationManager.NavigateTo($"/users/{id}");
    }

    public void NavEditor(Guid? id)
    {
        navigationManager.NavigateTo($"/users/editor/{id?.ToString() ?? ""}");
    }

    public async Task Delete(Guid id)
    {
        await DeleteAsync(id);
        StateHasChanged();
    }
}
