using Moq;
using IW5Forms.Api.DAL.Common.Repositories;
using AutoMapper;
using IW5Forms.Api.BL.Facades;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Models.Answer;

namespace IW5Forms.Api.BL.UnitTests;
public class AnswerFacadeTests
{
    [Fact]
    public void GetAll_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IAnswerRepository>();
        repoMock.Setup(answerRepo => answerRepo.GetAll());

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new AnswerFacade(repo, mapper);

        // Act
        facade.GetAll();

        // Assert
        repoMock.Verify(answerRepo => answerRepo.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IAnswerRepository>();
        repoMock.Setup(answerRepo => answerRepo.GetById(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new AnswerFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.GetById(itemId);

        // Assert
        repoMock.Verify(answerRepo => answerRepo.GetById(itemId), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Exists_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IAnswerRepository>();
        repoMock.Setup(answerRepo => answerRepo.Exists(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new AnswerFacade(repo, mapper);

        var answerModel = new AnswerListAndDetailModel
        {
            Id = Guid.NewGuid(),
            Text = "Answer text",
            FormResponder = Guid.NewGuid(),
        };

        // Act
        facade.CreateOrUpdate(answerModel);

        // Assert
        repoMock.Verify(
            answerRepo => answerRepo.Exists(answerModel.Id),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Create_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var answerModel = new AnswerListAndDetailModel
        {
            Id = Guid.NewGuid(),
            Text = "Answer text",
            FormResponder = Guid.NewGuid(),
        };
        var answerEntity = new AnswerEntity
        {
            Id = answerModel.Id,
            Text = answerModel.Text,
            ResponderId = answerModel.FormResponder,
            QuestionId = Guid.NewGuid(),
        };

        var repoMock = new Mock<IAnswerRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(answerRepo => answerRepo.Exists(It.IsAny<Guid>()))
            .Returns(false);
        mapperMock
            .Setup(mapper => mapper.Map<AnswerEntity>(
                It.IsAny<AnswerListAndDetailModel>()
            ))
            .Returns(answerEntity);

        var facade = new AnswerFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(answerModel);

        // Assert
        mapperMock.Verify(
            mapper => mapper.Map<AnswerEntity>(answerModel),
            Times.Once
        );
        repoMock.Verify(
            answerRepo => answerRepo.Insert(answerEntity),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Update_When_Entity_Exists()
    {
        // Arrange
        var answerModel = new AnswerListAndDetailModel
        {
            Id = Guid.NewGuid(),
            Text = "Answer text",
            FormResponder = Guid.NewGuid(),
        };
        var answerEntity = new AnswerEntity
        {
            Id = answerModel.Id,
            Text = answerModel.Text,
            ResponderId = answerModel.FormResponder,
            QuestionId = Guid.NewGuid(),
        };

        var repoMock = new Mock<IAnswerRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(answerRepo => answerRepo.Exists(It.IsAny<Guid>()))
            .Returns(true);
        mapperMock
            .Setup(mapper => mapper.Map<AnswerEntity>(
                It.IsAny<AnswerListAndDetailModel>()
            ))
            .Returns(answerEntity);
        repoMock
            .Setup(answerRepo => answerRepo.Update(It.IsAny<AnswerEntity>()))
            .Returns(Guid.NewGuid());

        var facade = new AnswerFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(answerModel);

        // Assert
        mapperMock.Verify(
            mapper => mapper.Map<AnswerEntity>(answerModel),
            Times.Once
        );
        repoMock.Verify(
            answerRepo => answerRepo.Update(answerEntity),
            Times.Once
        );
    }

    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IAnswerRepository>(MockBehavior.Strict);
        repoMock.Setup(answerRepo => answerRepo.Remove(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new AnswerFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.Delete(itemId);

        // Assert
        repoMock.Verify(answerRepo => answerRepo.Remove(itemId), Times.Once);
    }
}
