using Moq;
using IW5Forms.Api.DAL.Common.Repositories;
using AutoMapper;
using IW5Forms.Api.BL.Facades;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.BL.UnitTests;
public class QuestionFacadeTests
{
    [Fact]
    public void GetAll_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IQuestionRepository>();
        repoMock.Setup(questionRepo => questionRepo.GetAll());

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new QuestionFacade(repo, mapper);

        // Act
        facade.GetAll();

        // Assert
        repoMock.Verify(questionRepo => questionRepo.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IQuestionRepository>();
        repoMock.Setup(questionRepo => questionRepo.GetById(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new QuestionFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.GetById(itemId);

        // Assert
        repoMock.Verify(
            questionRepo => questionRepo.GetById(itemId),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Exists_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IQuestionRepository>();
        repoMock.Setup(questionRepo => questionRepo.Exists(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new QuestionFacade(repo, mapper);

        var questionModel = new QuestionDetailModel
        {
            Id = Guid.NewGuid(),
            QuestionType = QuestionTypes.TextAnswer,
            Text = "Question text",
            FormId = Guid.NewGuid(),
        };

        // Act
        facade.CreateOrUpdate(questionModel);

        // Assert
        repoMock.Verify(
            questionRepo => questionRepo.Exists(questionModel.Id),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Create_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var questionModel = new QuestionDetailModel
        {
            Id = Guid.NewGuid(),
            QuestionType = QuestionTypes.TextAnswer,
            Text = "Question text",
            FormId = Guid.NewGuid(),
        };
        var questionEntity = new QuestionEntity
        {
            Id = questionModel.Id,
            QuestionType = questionModel.QuestionType,
            Text = questionModel.Text,
            FormId = Guid.NewGuid(),
        };

        var repoMock = new Mock<IQuestionRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(questionRepo => questionRepo.Exists(It.IsAny<Guid>()))
            .Returns(false);
        repoMock
            .Setup(questionRepo => questionRepo.Insert(
                It.IsAny<QuestionEntity>()
            ))
            .Returns(Guid.NewGuid());

        var facade = new QuestionFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(questionModel);

        // Assert
        repoMock.Verify(
            questionRepo => questionRepo.Insert(It.IsAny<QuestionEntity>()),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Update_When_Entity_Exists()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var questionModel = new QuestionDetailModel
        {
            Id = questionId,
            QuestionType = QuestionTypes.TextAnswer,
            Text = "Question text",
            FormId = Guid.NewGuid(),
        };
        var questionEntity = new QuestionEntity
        {
            Id = questionModel.Id,
            QuestionType = questionModel.QuestionType,
            Text = questionModel.Text,
            FormId = Guid.NewGuid(),
        };

        var repoMock = new Mock<IQuestionRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(questionRepo => questionRepo.Exists(It.IsAny<Guid>()))
            .Returns(true);
        repoMock
            .Setup(questionRepo => questionRepo.Update(
                It.IsAny<QuestionEntity>()
            ))
            .Returns(questionId);
        repoMock
            .Setup(questionRepo => questionRepo.GetById(It.IsAny<Guid>()))
            .Returns(questionEntity);

        var facade = new QuestionFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(questionModel);

        // Assert
        repoMock.Verify(
            questionRepo => questionRepo.Update(questionEntity),
            Times.Once
        );
    }

    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IQuestionRepository>(MockBehavior.Strict);
        repoMock.Setup(questionRepo => questionRepo.Remove(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new QuestionFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.Delete(itemId);

        // Assert
        repoMock.Verify(
            questionRepo => questionRepo.Remove(itemId),
            Times.Once
        );
    }
}
