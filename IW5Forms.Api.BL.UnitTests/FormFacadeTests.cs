using Moq;
using IW5Forms.Api.DAL.Common.Repositories;
using AutoMapper;
using IW5Forms.Api.BL.Facades;
using IW5Forms.Common.Models.Form;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.UnitTests;
public class FormFacadeTests
{
    [Fact]
    public void GetAll_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IFormRepository>();
        repoMock.Setup(formRepo => formRepo.GetAll());

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new FormFacade(repo, mapper);

        // Act
        facade.GetAll();

        // Assert
        repoMock.Verify(formRepo => formRepo.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IFormRepository>();
        repoMock.Setup(formRepo => formRepo.GetById(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new FormFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.GetById(itemId);

        // Assert
        repoMock.Verify(formRepo => formRepo.GetById(itemId), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Exists_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IFormRepository>();
        repoMock.Setup(formRepo => formRepo.Exists(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new FormFacade(repo, mapper);

        var formModel = new FormDetailModel
        {
            Id = Guid.NewGuid(),
            AnswerAcceptanceStartTime = DateTime.UtcNow,
            AnswerAcceptanceEndTime = DateTime.UtcNow.AddDays(1),
        };

        // Act
        facade.CreateOrUpdate(formModel);

        // Assert
        repoMock.Verify(formRepo => formRepo.Exists(formModel.Id), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Create_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var formModel = new FormDetailModel
        {
            Id = Guid.NewGuid(),
            AnswerAcceptanceStartTime = DateTime.UtcNow,
            AnswerAcceptanceEndTime = DateTime.UtcNow.AddDays(1),
        };
        var formEntity = new FormEntity
        {
            Id = formModel.Id,
            Name = "Test Form",
            BeginTime = formModel.AnswerAcceptanceStartTime,
            EndTime = formModel.AnswerAcceptanceEndTime,
            Incognito = false,
            SingleTry = true,
        };

        var repoMock = new Mock<IFormRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(formRepo => formRepo.Exists(It.IsAny<Guid>()))
            .Returns(false);
        mapperMock
            .Setup(mapper => mapper.Map<FormEntity>(
                It.IsAny<FormDetailModel>()
            ))
            .Returns(formEntity);

        var facade = new FormFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(formModel);

        // Assert
        mapperMock.Verify(
            mapper => mapper.Map<FormEntity>(formModel),
            Times.Once
        );
        repoMock.Verify(
            formRepo => formRepo.Insert(formEntity),
            Times.Once
        );
    }

    [Fact]
    public void CreateOrUpdate_Calls_Update_When_Entity_Exists()
    {
        // Arrange
        var formModel = new FormDetailModel
        {
            Id = Guid.NewGuid(),
            AnswerAcceptanceStartTime = DateTime.UtcNow,
            AnswerAcceptanceEndTime = DateTime.UtcNow.AddDays(1),
        };
        var formEntity = new FormEntity
        {
            Id = formModel.Id,
            Name = "Test Form",
            BeginTime = formModel.AnswerAcceptanceStartTime,
            EndTime = formModel.AnswerAcceptanceEndTime,
            Incognito = false,
            SingleTry = true,
        };

        var repoMock = new Mock<IFormRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(formRepo => formRepo.Exists(It.IsAny<Guid>()))
            .Returns(true);
        mapperMock
            .Setup(mapper => mapper.Map<FormEntity>(
                It.IsAny<FormDetailModel>()
            ))
            .Returns(formEntity);
        repoMock
            .Setup(formRepo => formRepo.Update(It.IsAny<FormEntity>()))
            .Returns(Guid.NewGuid());

        var facade = new FormFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(formModel);

        // Assert
        mapperMock.Verify(
            mapper => mapper.Map<FormEntity>(formModel),
            Times.Once
        );
        repoMock.Verify(formRepo => formRepo.Update(formEntity), Times.Once);
    }

    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IFormRepository>(MockBehavior.Strict);
        repoMock.Setup(formRepo => formRepo.Remove(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new FormFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.Delete(itemId);

        // Assert
        repoMock.Verify(formRepo => formRepo.Remove(itemId), Times.Once);
    }
}
