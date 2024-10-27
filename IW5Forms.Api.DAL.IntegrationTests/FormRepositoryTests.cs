using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.DAL.IntegrationTests;

public class FormRepositoryTests
{
    private readonly IDatabaseFixture dbFixture;

    public FormRepositoryTests()
    {
        dbFixture = new InMemoryDatabaseFixture();
    }

    [Fact]
    public void GetById_Returns_Requested_Form_Including_Questions()
    {
        // Arrange
        var formRepo = dbFixture.GetFormRepository();

        // Act
        var form = formRepo.GetById(dbFixture.FormGuids[0]);

        // Assert
        Assert.NotNull(form);
        Assert.Equal(dbFixture.FormGuids[0], form.Id);
        Assert.Equal("Fruit and vegetables", form.Name);

        Assert.Equal(2, form.Questions.Count);

        var question1 = Assert.Single(
            form.Questions.Where(q => q.Id == dbFixture.QuestionGuids[0])
        );
        Assert.Equal(dbFixture.FormGuids[0], question1.FormId);

        var question2 = Assert.Single(
            form.Questions.Where(q => q.Id == dbFixture.QuestionGuids[1])
        );
        Assert.Equal(dbFixture.FormGuids[0], question2.FormId);

        Assert.Equal(2, question1.Answers.Count);
        var answer1 = Assert.Single(
            question1.Answers.Where(a => a.Id == dbFixture.AnswerGuids[0])
        );
        Assert.Equal(dbFixture.QuestionGuids[0], answer1.QuestionId);
        Assert.Equal("Apple", answer1.Text);

        Assert.Equal(2, question2.Answers.Count);
        var answer2 = Assert.Single(
            question2.Answers.Where(a => a.Id == dbFixture.AnswerGuids[3])
        );
        Assert.Equal(dbFixture.QuestionGuids[1], answer2.QuestionId);
        Assert.Equal("Cherry", answer2.Text);
    }

    [Fact]
    public void Insert_Saves_Form_And_Questions()
    {
        // Arrange
        var formRepo = dbFixture.GetFormRepository();

        var formId = Guid.NewGuid();
        var formName = "Test form";
        var beginTime = DateTime.UtcNow.AddDays(2);

        var questionId = Guid.NewGuid();
        var questionType = QuestionTypes.NumericValue;

        var newForm = new FormEntity {
            Id = formId,
            Name = formName,
            BeginTime = beginTime,
            EndTime = DateTime.UtcNow.AddDays(9),
            Incognito = true,
            SingleTry = true,
            Questions = new List<QuestionEntity>()
            {
                new QuestionEntity
                {
                    Id = questionId,
                    QuestionType = questionType,
                    Text = "What is your favourite number?",
                    Description = "Integer number only",
                    FormId = formId,
                }
            }
        };

        // Act
        formRepo.Insert(newForm);

        // Assert
        var form = dbFixture.GetFormDirectly(formId);
        Assert.NotNull(form);
        Assert.Equal("Test form", form.Name);
        Assert.Equal(beginTime, form.BeginTime);

        var question = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(question);
        Assert.Equal(questionType, question.QuestionType);
    }

    [Fact]
    public void Update_Saves_New_Question()
    {
        // Arrange
        var formRepo = dbFixture.GetFormRepository();

        var formId = dbFixture.FormGuids[0];
        var form = dbFixture.GetFormDirectly(formId);

        var questionId = Guid.NewGuid();
        var questionType = QuestionTypes.TextAnswer;

        var newQuestion = new QuestionEntity
        {
            Id = questionId,
            QuestionType = questionType,
            Text = "Is this a question?",
            FormId = formId,
        };

        // Act
        form!.Questions.Add(newQuestion);
        formRepo.Update(form);

        // Assert
        var formFromDb = dbFixture.GetFormDirectly(formId);
        Assert.NotNull(formFromDb);
        Assert.Single(formFromDb.Questions.Where(q => q.Id == questionId));

        var question = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(question);
        Assert.Equal(questionType, question.QuestionType);
    }

    [Fact]
    public void Update_Updates_Question()
    {
        // Arrange
        var formRepo = dbFixture.GetFormRepository();

        var formId = dbFixture.FormGuids[0];
        var form = dbFixture.GetFormDirectly(formId);
        var question = form!.Questions.First();
        var questionId = question.Id;

        var text = "Is this the edited question?";

        // Act
        question.Text = text;
        formRepo.Update(form);

        // Assert
        var formFromDb = dbFixture.GetFormDirectly(formId);
        Assert.NotNull(formFromDb);

        var questionFromDb = dbFixture.GetQuestionDirectly(questionId);
        Assert.NotNull(questionFromDb);
        Assert.Equal(text, questionFromDb.Text);
    }


    [Fact]
    public void Update_Removes_Questions()
    {
        // Arrange
        var formRepo = dbFixture.GetFormRepository();

        var formId = dbFixture.FormGuids[0];
        var form = dbFixture.GetFormDirectly(formId);
        var questionId = form!.Questions.First().Id;

        // Act
        form.Questions.Clear();
        formRepo.Update(form);

        // Assert
        var formFromDb = dbFixture.GetFormDirectly(formId);
        Assert.NotNull(formFromDb);
        Assert.Empty(formFromDb.Questions);

        var questionFromDb = dbFixture.GetQuestionDirectly(questionId);
        Assert.Null(questionFromDb);
    }
}
