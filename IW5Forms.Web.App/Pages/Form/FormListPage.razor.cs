using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;


public partial class FormListPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;

    [Parameter]
    public string? Action { get; init; } = null;

    private ICollection<FormListModel> Forms { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadForms();
        navigationManager.LocationChanged += HandleRedirect;
        await base.OnInitializedAsync();
    }

    private async void HandleRedirect(object sender, LocationChangedEventArgs e)
    {
        await LoadForms();
        StateHasChanged();
    }

    private async Task LoadForms() {
        switch (Action ?? "all") {
            case "all":
                Forms = await FormFacade.GetAllAsync();
                break;
            case "managable":
                Forms = await FormFacade.GetManagableAsync();
                break;
            default:
                navigationManager.NavigateTo("404");
                break;
        }
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

    private void NavFormDetail(
        TableRowClickEventArgs<FormListModel> args
    ) {
        navigationManager.NavigateTo($"/form/id/{args.Item!.Id}");
    }

    public void NavAnswers(Guid id)
    {
        navigationManager.NavigateTo($"/form/answers/{id}");
    }

    public void NavEditor(Guid? id)
    {
        navigationManager.NavigateTo($"/form/editor/{id?.ToString() ?? ""}");
    }

    public async Task Delete(Guid id)
    {
        await FormFacade.DeleteAsync(id);
        Forms = Forms.Where(f => f.Id != id).ToList();
        StateHasChanged();
    }
}
