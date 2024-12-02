using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

public partial class FormListPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;

    private ICollection<FormListModel> Forms { get; set; } =
        new List<FormListModel>();

    protected override async Task OnInitializedAsync()
    {
        Forms = await FormFacade.GetAllAsync();

        await base.OnInitializedAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        var parameters = new DialogParameters<Dialog>
        {
            { x => x.ContentText, "Do you really want to delete this form?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error },
            { x => x.OnSubmit, EventCallback.Factory.Create(
                this, async () => await Delete(id)
            )},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

        return DialogService.ShowAsync<Dialog>("Delete", parameters, options);
    }

    public void NavDetail(Guid id)
    {
        navigationManager.NavigateTo($"/form/{id}");
    }

    public void NavEditor(Guid? id)
    {
        navigationManager.NavigateTo($"/form/editor/{id?.ToString() ?? ""}");
    }

    public async Task Delete(Guid id)
    {
        await FormFacade.DeleteAsync(id);
        StateHasChanged();
    }
}
