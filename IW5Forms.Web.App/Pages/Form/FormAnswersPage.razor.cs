using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

public partial class FormAnswersPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    private FormDetailModel Data { get; set; } = null!;
    private List<QuestionDetailModel> Questions { get; set; } =
        new List<QuestionDetailModel>();

    private string OpenRange =>
        $"{Data.BeginTime:dd.MM.yyyy HH:mm} " +
        $"- {Data.EndTime:dd.MM.yyyy HH:mm}";

    private Dictionary<Guid, object?> Answers { get; set; } = new();

    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        Data = await FormFacade.GetByIdAsync(Id);
        if (Data is not null) {
            var questions = Data.Questions.Select(
                q => QuestionFacade.GetByIdAsync(q.Id)
            );
            Questions = (await Task.WhenAll(questions)).ToList();
        }

        await base.OnInitializedAsync();
    }

    private void NavQuestionDetail(
        TableRowClickEventArgs<QuestionDetailModel> args
    ) {
        navigationManager.NavigateTo($"/question/{args.Item.Id}");
    }
}
