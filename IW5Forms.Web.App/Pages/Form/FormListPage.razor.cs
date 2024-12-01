using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

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

    public void NavDetail(Guid id)
    {
        navigationManager.NavigateTo($"/form/{id}");
    }

    public void NavEditor(Guid? id)
    {
        navigationManager.NavigateTo($"/form/editor/{id?.ToString() ?? ""}");
    }
}
