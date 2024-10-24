using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.DAL.IntegrationTests;

public class QuestionRepositoryTests
{
    private readonly IDatabaseFixture dbFixture;

    public QuestionRepositoryTests()
    {
        dbFixture = new InMemoryDatabaseFixture();
    }

    [Fact]
    public void GetById_Returns_Requested_Question_Including_Answers()
    {
        // Arrange
        var questionRepo = dbFixture.GetQuestionRepository();

        // Act
        var question = questionRepo.GetById(dbFixture.QuestionGuids[0]);

        // Assert
        Assert.NotNull(question);
        Assert.Equal(dbFixture.QuestionGuids[0], question.Id);
        Assert.Equal("What company is called after fruit?", question.Text);

        var answer = Assert.Single(
            question.Answers.Where(a => a.Id == dbFixture.AnswerGuids[1])
        );
        Assert.Equal(dbFixture.QuestionGuids[0], answer.QuestionId);
        Assert.Equal("Carrot", answer.Text);
    }

    [Fact]
    public void Insert_Saves_Question_And_Answer()
    {
        // Arrange
        var questionRepo = dbFixture.GetQuestionRepository();

        var questionId = Guid.NewGuid();
        var questionText = "Is this the inserted question?";

        var answerId = Guid.NewGuid();
        var answerText = "I hope so";

        var newQuestion = new QuestionEntity {
            Id = questionId,
            QuestionType = QuestionTypes.TextAnswer,
            Text = questionText,
            FormId = dbFixture.FormGuids[0],
            Answers = new List<AnswerEntity>
            {
                new AnswerEntity
                {
                    Id = answerId,
                    Text = answerText,
                    ResponderId = dbFixture.UserGuids[0],
                    QuestionId = questionId,
                }
            }
        };

        // Act
        questionRepo.Insert(newQuestion);

        // Assert
        var question = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(question);
        Assert.Equal(questionId, question.Id);
        Assert.Equal(questionText, question.Text);

        var answer = dbFixture.GetAnswerDirectly(answerId);
        Assert.NotNull(answer);
        Assert.Equal(answerId, answer.Id);
        Assert.Equal(answerText, answer.Text);
    }

    [Fact]
    public void Update_Saves_New_Answer()
    {
        // Arrange
        var questionRepo = dbFixture.GetQuestionRepository();

        var questionId = dbFixture.QuestionGuids[0];
        var question = dbFixture.GetQuestionDirectly(questionId);

        var answerId = Guid.NewGuid();
        var answerText = "This is new answer";

        var newAnswer = new AnswerEntity
        {
            Id = answerId,
            Text = answerText,
            ResponderId = dbFixture.UserGuids[0],
            QuestionId = questionId,
        };

        // Act
        question!.Answers.Add(newAnswer);
        questionRepo.Update(question);

        // Assert
        var questionFromDb = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(questionFromDb);
        Assert.Single(questionFromDb.Answers.Where(a => a.Id == answerId));

        var answer = dbFixture.GetAnswerDirectly(answerId);
        Assert.NotNull(answer);
        Assert.Equal(answerId, answerId);
        Assert.Equal(answerText, answer.Text);
    }

    [Fact]
    public void Update_Updates_Answer()
    {
        // Arrange
        var questionRepo = dbFixture.GetQuestionRepository();

        var questionId = dbFixture.QuestionGuids[0];
        var question = dbFixture.GetQuestionDirectly(questionId);

        var answer = question!.Answers.First();
        var answerId = answer.Id;

        var text = "This is the edited answer";

        // Act
        answer.Text = text;
        questionRepo.Update(question);

        // Assert
        var questionFromDb = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(questionFromDb);

        var answerFromDb = dbFixture.GetAnswerDirectly(answerId);
        Assert.NotNull(answerFromDb);
        Assert.Equal(text, answerFromDb.Text);
    }


    [Fact]
    public void Update_Removes_Answers()
    {
        // Arrange
        var questionRepo = dbFixture.GetQuestionRepository();

        var questionId = dbFixture.QuestionGuids[0];
        var question = dbFixture.GetQuestionDirectly(questionId);
        var answerId = question!.Answers.First().Id;

        // Act
        question.Answers.Clear();
        questionRepo.Update(question);

        // Assert
        var questionFromDb = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(questionFromDb);
        Assert.Empty(questionFromDb.Answers);

        var answerFromDb = dbFixture.GetAnswerDirectly(answerId);
        Assert.Null(answerFromDb);
    }
}
