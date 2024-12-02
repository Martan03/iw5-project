using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Question;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class QuestionEditPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    private QuestionDetailModel Data { get; set; } = null!;

    [Parameter]
    public string Action { get; init; } = null!;
    [Parameter]
    public Guid Id { get; init; }

    protected override async Task OnInitializedAsync()
    {
        if (Action == "create") {
            Data = GetNewQuestion(Id);
        } else if (Action == "edit") {
            Data = await QuestionFacade.GetByIdAsync(Id);
        } else {
            navigationManager.NavigateTo("404");
        }

        await base.OnInitializedAsync();
    }

    public async Task Save()
    {
        await QuestionFacade.SaveAsync(Data);
        navigationManager.NavigateTo($"/form/{Data.FormId}");
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
}
