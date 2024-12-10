using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class QuestionDetailPage
{
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    private QuestionDetailModel Data { get; set; } = null!;

    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        Data = await QuestionFacade.GetByIdAsync(Id);

        await base.OnInitializedAsync();
    }
}
