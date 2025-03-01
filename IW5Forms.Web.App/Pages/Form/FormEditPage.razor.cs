using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

public partial class FormEditPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    private FormDetailModel Data { get; set; } = GetNewForm();
    private List<QuestionDetailModel> Questions { get; set; } = new();

    private MudForm Form { get; set; } = null!;

    [Parameter]
    public Guid Id { get; init; }

    public string? TimeErrorMessage { get; set; } = null;

    private DateTime? BeginDate
    {
        get => Data.BeginTime;
        set => Data.BeginTime = value ?? Data.BeginTime;
    }

    private TimeSpan? BeginTime
    {
        get => Data.BeginTime.TimeOfDay;
        set => Data.BeginTime = value is not null ?
            Data.BeginTime.Date.Add(value.Value) : Data.BeginTime;
    }

    private DateTime? EndDate
    {
        get => Data.EndTime;
        set => Data.EndTime = value ?? Data.EndTime;
    }

    private TimeSpan? EndTime
    {
        get => Data.EndTime.TimeOfDay;
        set => Data.EndTime = value is not null ?
            Data.EndTime.Date.Add(value.Value) : Data.EndTime;
    }

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            Data = await FormFacade.GetByIdAsync(Id);
            if (Data != null)
                Questions = await LoadQuestions();
        }

        await base.OnInitializedAsync();
    }

    public async Task Save()
    {
        CheckTimeCorrect();
        if (TimeErrorMessage != null)
            return;

        await Form.Validate();
        if (!Form.IsValid) {
            return;
        }

        await FormFacade.SaveAsync(Data);
        foreach (var question in Questions) {
            await QuestionFacade.SaveAsync(question);
        }
        navigationManager.NavigateTo($"/forms");
    }

    public async Task Delete()
    {
        await FormFacade.DeleteAsync(Id);
        navigationManager.NavigateTo($"/forms");
    }

    private static FormDetailModel GetNewForm() => new() {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        BeginTime = DateTime.Now,
        EndTime = DateTime.Now.AddDays(30),
        Incognito = false,
        SingleTry = true,
    };

    private async Task<List<QuestionDetailModel>> LoadQuestions() {
        var questions = new List<QuestionDetailModel>();
        foreach (var question in Data.Questions) {
            var quest = await QuestionFacade.GetByIdAsync(question.Id);
            questions.Add(quest);
        }
        return questions;
    }

    public void AddQuestion()
    {
        Questions.Add(GetNewQuestion(Data.Id));
        StateHasChanged();
    }

    public void RemQuestion(int id)
    {
        Questions.RemoveAt(id);
        StateHasChanged();
    }

    public void AddOption(QuestionDetailModel question)
    {
        question.Options.Add(string.Empty);
        StateHasChanged();
    }

    public void RemOption(QuestionDetailModel question, int id)
    {
        question.Options.RemoveAt(id);
        StateHasChanged();
    }

    public static string TypeToString(QuestionTypes type) {
        return type switch
        {
            QuestionTypes.ManyOptions => "Multiple options",
            QuestionTypes.TextAnswer => "Text",
            QuestionTypes.NumericValue => "Numeric",
            _ => type.ToString(),
        };
    }

    private static QuestionDetailModel GetNewQuestion(Guid formId) => new() {
        Id = Guid.NewGuid(),
        QuestionType = QuestionTypes.TextAnswer,
        Text = string.Empty,
        FormId = formId,
    };

    private void IncognitoClick()
    {
        if (Data is { Incognito: false, SingleTry: true }) Data.SingleTry = false;
    }

    private void SingleTryClick()
    {
        if (Data is { Incognito: true, SingleTry: false }) Data.Incognito = false;
    }

    private void CheckTimeCorrect()
    {
        if (BeginDate.Value.Date == EndDate.Value.Date && EndTime < BeginTime)
        {
            TimeErrorMessage = "Begin time is after end time!";
        }
        else if (BeginDate > EndDate)
        {
            TimeErrorMessage = "Begin date is after end date!";
        }
        else TimeErrorMessage = null;
    }
}
