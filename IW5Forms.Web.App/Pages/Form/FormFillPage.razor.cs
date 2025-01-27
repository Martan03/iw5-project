using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using System.Security.Claims;

namespace IW5Forms.Web.App.Pages;

public partial class FormFillPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;
    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private FormDetailModel Data { get; set; } = null!;

    private MudForm Form { get; set; } = null!;

    private string OpenRange =>
        $"{Data.BeginTime:dd.MM.yyyy HH:mm} " +
        $"- {Data.EndTime:dd.MM.yyyy HH:mm}";

    private Dictionary<Guid, object?> Answers { get; set; } = new();

    [Parameter]
    public Guid Id { get; init; }

    public bool LoadForm = false;

    protected override async Task OnInitializedAsync()
    {
        Data = await FormFacade.GetByIdAsync(Id);

        LoadForm = Data.Incognito;

        if (!LoadForm)
        {

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var authenticated = authState.User.Identity?.IsAuthenticated ?? false;
            LoadForm = authenticated;
        }

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
        await Form.Validate();
        if (!Form.IsValid)
        {
            return;
        }

        foreach (var question in Data.Questions)
        {
            if (Answers.TryGetValue(question.Id, out var answer))
            {
                var answerModel = new AnswerListAndDetailModel()
                {
                    Id = Guid.NewGuid(),
                    Text = answer?.ToString() ?? "",
                    QuestionId = question.Id,
                };
                await AnswerFacade.SaveAsync(answerModel);
            }
        }
        navigationManager.NavigateTo($"/forms");
    }
}
