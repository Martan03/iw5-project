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
        };

        // Act
        facade.CreateOrUpdate(answerModel, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString(), true);

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
        };
        var answerEntity = new AnswerEntity
        {
            Id = answerModel.Id,
            Text = answerModel.Text,
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
        facade.CreateOrUpdate(answerModel, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString(), true);

        // Assert
        repoMock.Verify(
            answerRepo => answerRepo.Insert(It.IsAny<AnswerEntity>()),
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
        };
        var answerEntity = new AnswerEntity
        {
            Id = answerModel.Id,
            Text = answerModel.Text,
            QuestionId = Guid.NewGuid(),
        };

        var repoMock = new Mock<IAnswerRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(answerRepo => answerRepo.Exists(It.IsAny<Guid>()))
            .Returns(true);
        repoMock
            .Setup(answerRepo => answerRepo.Update(It.IsAny<AnswerEntity>()))
            .Returns(Guid.NewGuid());
        repoMock
            .Setup(answerRepo => answerRepo.GetById(It.IsAny<Guid>()))
            .Returns(answerEntity);

        var answerMock = new Mock<AnswerFacade>(repoMock.Object, mapperMock.Object) { CallBase = true };
        answerMock.Setup(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()));

        // Act
        answerMock.Object.CreateOrUpdate(answerModel, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString(), false);

        // Assert
        answerMock.Verify(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()), Times.Once);
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
