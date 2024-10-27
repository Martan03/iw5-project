
using System.Net;
using System.Net.Http.Json;
using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using Microsoft.Extensions.ObjectPool;

namespace IW5Forms.Api.App.EndToEndTests;

public class QuestionControllerTests : IAsyncDisposable
{
    private readonly IW5FormsApiApplicationFactory app;
    private readonly Lazy<HttpClient> client;

    public QuestionControllerTests()
    {
        app = new IW5FormsApiApplicationFactory();
        client = new Lazy<HttpClient>(app.CreateClient());
    }

    public async ValueTask DisposeAsync()
    {
        await app.DisposeAsync();
    }

    [Fact]
    public async void GetAllQuestions_Returns_At_Least_One_Form()
    {
        var response = await client.Value.GetAsync("/api/question");

        response.EnsureSuccessStatusCode();

        var questions = await response
            .Content
            .ReadFromJsonAsync<ICollection<QuestionListModel>>();
        Assert.NotNull(questions);
        Assert.NotEmpty(questions);
    }

    [Fact]
    public async void GetQuestionById_Returns_Form_If_Exists()
    {
        var questionId = new Guid("0d4fa150-ad80-4d46-a511-4c666166ec5e");

        var response =
            await client.Value.GetAsync($"/api/question/{questionId}");
        response.EnsureSuccessStatusCode();

        var question =
            await response.Content.ReadFromJsonAsync<QuestionListModel>();
        Assert.NotNull(question);
        Assert.Equal(questionId, question.Id);
    }

    [Fact]
    public async void GetQuestionById_Returns_Not_Found_If_Does_Not_Exist()
    {
        var questionId = Guid.NewGuid();

        var response =
            await client.Value.GetAsync($"/api/question/{questionId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void CreateQuestion_Create_New_Form()
    {
        var newQuestion = new QuestionDetailModel
        {
            Id = Guid.NewGuid(),
            QuestionType = QuestionTypes.TextAnswer,
            Text = "Is this the new question that I just created?",
            Description = "This is part of the EndToEnd testing",
        };

        var response =
            await client.Value.PostAsJsonAsync("/api/question", newQuestion);
        response.EnsureSuccessStatusCode();

        var createdQuestion =
            await response.Content.ReadFromJsonAsync<QuestionDetailModel>();
        Assert.NotNull(createdQuestion);
        Assert.Equal(newQuestion.Id, createdQuestion.Id);
        Assert.Equal(
            newQuestion.Text,
            createdQuestion.Text
        );
    }

    [Fact]
    public async void UpdateQuestion_Updates_Form_Details()
    {
        var questionId = new Guid("0d4fa150-ad80-4d46-a511-4c666166ec5e");
        var updatedQuestion = new QuestionDetailModel
        {
            Id = questionId,
            QuestionType = QuestionTypes.TextAnswer,
            Text = "Is this the new question that I just created?",
            Description = "This is part of the EndToEnd testing",
        };

        var response = await client.Value.PutAsJsonAsync(
            $"/api/question/{questionId}",
            updatedQuestion
        );
        response.EnsureSuccessStatusCode();

        var question =
            await response.Content.ReadFromJsonAsync<QuestionDetailModel>();
        Assert.NotNull(question);
        Assert.Equal(updatedQuestion.Text, question.Text);
    }

    [Fact]
    public async void DeleteQuestion_Deletes_Existing_Form()
    {
        var questionId = new Guid("0d4fa150-ad80-4d46-a511-4c666166ec5e");

        var response =
            await client.Value.DeleteAsync($"/api/question/{questionId}");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse =
            await client.Value.GetAsync($"/api/question/{questionId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
