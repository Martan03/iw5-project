using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
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
    private QuestionFacade QuestionFacade { get; set; } = null!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private FormDetailModel Data { get; set; } = null!;

    private MudForm Form { get; set; } = null!;
    private List<QuestionDetailModel> Questions { get; set; } = new();


    private string OpenRange =>
        $"{Data.BeginTime:dd.MM.yyyy HH:mm} " +
        $"- {Data.EndTime:dd.MM.yyyy HH:mm}";

    private Dictionary<Guid, object?> Answers { get; set; } = new();

    [Parameter]
    public Guid Id { get; init; }

    public bool LoadForm = false;
    public bool UserCanAddQuestions = false;
    public bool AlreadyFilledSingleTry = false;
    public string? IdentityUsername = null;
    public bool? IdentityIsAdmin = null;


    protected override async Task OnInitializedAsync()
    {
        Data = await FormFacade.GetByIdAsync(Id);

        LoadForm = Data.Incognito;
        if (!LoadForm)
        {
            GetIdentityUsername(AuthenticationStateProvider);
            LoadForm = IdentityUsername != null;

            CanAddQuestions(Data.IdentityOwnerId);
            if (Data.SingleTry)
            {
                CheckAlreadyFilled();
            }
            Questions = await LoadQuestions();
        }


        await base.OnInitializedAsync();
    }

    private async void GetIdentityUsername(AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userClaims = authState?.User.Claims;
        foreach (var userClaim in userClaims)
        {
            if (userClaim.Type == "username")
            {
                IdentityUsername = userClaim.Value;

            }
            if (userClaim is { Type: "role", Value: "admin" })
            {
                IdentityIsAdmin = true;

            }
        }

    }

    private void CheckAlreadyFilled()
    {
        if (Data.CompletedUsersId.Contains(IdentityUsername)) AlreadyFilledSingleTry = true;

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

        
        if (IdentityUsername != null)
        {
            Data.CompletedUsersId.Add(IdentityUsername);
            await FormFacade.SaveAsync(Data);
            foreach (var question in Questions)
            {
                await QuestionFacade.SaveAsync(question);
            }

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

    private async Task<List<QuestionDetailModel>> LoadQuestions()
    {
        var questions = new List<QuestionDetailModel>();
        foreach (var question in Data.Questions)
        {
            var quest = await QuestionFacade.GetByIdAsync(question.Id);
            questions.Add(quest);
        }
        return questions;
    }

    private void CanAddQuestions(string? formOwnerId)
    {
        UserCanAddQuestions = IdentityIsAdmin == true || IdentityUsername == formOwnerId;
    }
}
