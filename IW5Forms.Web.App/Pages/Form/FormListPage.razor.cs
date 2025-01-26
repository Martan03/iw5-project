using System.Net.Http.Json;
using System.Text.RegularExpressions;
using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

[Authorize]
public partial class FormListPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;

    [Inject]
    private HttpClient httpClient { get; set; } = null!;

    [Parameter]
    public string Action { get; init; } = "all";

    private ICollection<FormListModel> Forms { get; set; } =
        new List<FormListModel>();

    protected override async Task OnInitializedAsync()
    {
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
        StateHasChanged();
    }
}
