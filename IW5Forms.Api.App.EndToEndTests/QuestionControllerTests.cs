
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
        // TODO: add seeded question ID
        var questionId = Guid.NewGuid();

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
    public async void CreateQuestion_Returns_BadRequest_If_Required_Missing()
    {
        var newQuestion = new
        {
            Id = Guid.NewGuid(),
        };

        var response =
            await client.Value.PostAsJsonAsync("/api/question", newQuestion);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void CreateQuestion_Returns_BadRequest_If_Data_Invalid()
    {
        var newQuestion = new
        {
            Id = Guid.NewGuid(),
            Type = QuestionTypes.TextAnswer,
            Text = "Is this the new question that I just created?",
            Description = "This is part of the EndToEnd testing",
            InvalidAttribute = "Why is this here?",
        };

        var response =
            await client.Value.PostAsJsonAsync("/api/question", newQuestion);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void UpdateQuestion_Updates_Form_Details()
    {
        // TODO: add seeded question ID
        var questionId = Guid.NewGuid();
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
    public async void UpdateQuestion_Returns_BadRequest_If_Required_Missing()
    {
        // TODO: add seeded question ID
        var questionId = Guid.NewGuid();
        var updatedQuestion = new
        {
            Id = questionId,
        };

        var response = await client.Value.PutAsJsonAsync(
            $"/api/question/{questionId}",
            updatedQuestion
        );
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void UpdateQuestion_Returns_BadRequest_If_Data_Invalid()
    {
        // TODO: add seeded question ID
        var questionId = Guid.NewGuid();
        var updatedQuestion = new
        {
            Id = questionId,
            Type = QuestionTypes.TextAnswer,
            Text = "Is this the new question that I just created?",
            Description = "This is part of the EndToEnd testing",
            InvalidAttribute = "Why is this here?",
        };

        var response = await client.Value.PutAsJsonAsync(
            $"/api/question/{questionId}",
            updatedQuestion
        );
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void UpdateQuestion_Returns_NotFound_If_Form_Does_Not_Exist()
    {
        var formId = Guid.NewGuid();
        var updatedQuestion = new
        {
            Id = formId,
            Type = QuestionTypes.TextAnswer,
            Text = "Is this the new question that I just created?",
            Description = "This is part of the EndToEnd testing",
            InvalidAttribute = "Why is this here?",
        };

        var response = await client.Value.PutAsJsonAsync(
            $"/api/question/{formId}",
            updatedQuestion
        );
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void DeleteQuestion_Deletes_Existing_Form()
    {
        // TODO: add seeded question ID
        var questionId = Guid.NewGuid();

        var response =
            await client.Value.DeleteAsync($"/api/question/{questionId}");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse =
            await client.Value.GetAsync($"/api/question/{questionId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async void DeleteQuestion_Returns_NotFound_If_Form_Does_Not_Exist()
    {
        var questionId = Guid.NewGuid();

        var response =
            await client.Value.DeleteAsync($"/api/question/{questionId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
