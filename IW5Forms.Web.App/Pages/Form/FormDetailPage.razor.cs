using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class FormDetailPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;

    private FormDetailModel Data { get; set; } = null!;

    private string OpenRange =>
        $"{Data.BeginTime:dd.MM.yyyy HH:mm} " +
        $"- {Data.EndTime:dd.MM.yyyy HH:mm}";

    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        Data = await FormFacade.GetByIdAsync(Id);

        await base.OnInitializedAsync();
    }

    public void NavQuestionCreate()
    {
        navigationManager.NavigateTo(
            $"/question/create/{Data.Id}"
        );
    }
}
