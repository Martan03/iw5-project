using IW5Forms.Common.Models.Answer;
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
    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    private FormDetailModel Data { get; set; } = null!;

    private string OpenRange =>
        $"{Data.BeginTime:dd.MM.yyyy HH:mm} " +
        $"- {Data.EndTime:dd.MM.yyyy HH:mm}";

    private Dictionary<Guid, object?> Answers { get; set; } = new();

    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        Data = await FormFacade.GetByIdAsync(Id);

        await base.OnInitializedAsync();
    }

    public void NavQuestionDetail(Guid id)
    {
        navigationManager.NavigateTo($"/question/{id}");
    }

    public void NavQuestionCreate()
    {
        navigationManager.NavigateTo(
            $"/question/create/{Data.Id}"
        );
    }

    private async void SubmitForm()
    {
        foreach (var question in Data.Questions)
        {
            if (Answers.TryGetValue(question.Id, out var answer))
            {
                var answerModel = new AnswerListAndDetailModel() {
                    Id = Guid.NewGuid(),
                    Text = answer.ToString(),
                    // TODO
                    ResponderId = Guid.NewGuid(),
                    QuestionId = question.Id,
                };
                await AnswerFacade.SaveAsync(answerModel);
            }
        }
        navigationManager.NavigateTo($"/forms");
    }
}
