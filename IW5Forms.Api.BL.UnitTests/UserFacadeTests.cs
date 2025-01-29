using Moq;
using IW5Forms.Api.DAL.Common.Repositories;
using AutoMapper;
using IW5Forms.Api.BL.Facades;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.BL.UnitTests;
public class UserFacadeTests
{
    [Fact]
    public void GetAll_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(userRepo => userRepo.GetAll());

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new UserFacade(repo, mapper);

        // Act
        facade.GetAll();

        // Assert
        repoMock.Verify(userRepo => userRepo.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>(MockBehavior.Loose);
        repoMock.Setup(userRepo => userRepo.GetById(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new UserFacade(repo, mapper);

        var itemId = Guid.NewGuid();

        // Act
        facade.GetById(itemId);

        // Assert
        repoMock.Verify(userRepo => userRepo.GetById(itemId), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Exists_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(userRepo => userRepo.Exists(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>().Object;
        var facade = new UserFacade(repo, mapper);

        var userModel = new UserDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "User Name",
            Role = RoleTypes.User,
        };

        // Act
        facade.CreateOrUpdate(userModel, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString(), true);

        // Assert
        repoMock.Verify(userRepo => userRepo.Exists(userModel.Id), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Create_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var userModel = new UserDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "User Name",
            Role = RoleTypes.User,
        };
        var userEntity = new UserEntity
        {
            Id = userModel.Id,
            Name = userModel.Name,
            Role = Common.Enums.RoleTypes.User,
        };

        var repoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(userRepo => userRepo.Exists(It.IsAny<Guid>()))
            .Returns(false);

        var facade = new UserFacade(repoMock.Object, mapperMock.Object);

        // Act
        facade.CreateOrUpdate(userModel, null, true);

        // Assert
        repoMock.Verify(userRepo => userRepo.Insert(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public void CreateOrUpdate_Calls_Update_When_Entity_Exists()
    {
        // Arrange
        var userModel = new UserDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "User Name",
            Role = RoleTypes.User,
        };
        var userEntity = new UserEntity
        {
            Id = userModel.Id,
            Name = userModel.Name,
            Role = Common.Enums.RoleTypes.User,
        };

        var repoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(userRepo => userRepo.Exists(It.IsAny<Guid>()))
            .Returns(true);
        repoMock
            .Setup(userRepo => userRepo.Update(It.IsAny<UserEntity>()))
            .Returns(Guid.NewGuid());

        var userMock = new Mock<UserFacade>(repoMock.Object, mapperMock.Object) { CallBase = true };
        userMock.Setup(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()));

        // Act
        userMock.Object.CreateOrUpdate(userModel, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString(), false);

        // Assert
        userMock.Verify(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()), Times.Once);
        repoMock.Verify(
            userRepo => userRepo.Update(It.IsAny<UserEntity>()),
            Times.Once
        );
    }

    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>(MockBehavior.Strict);
        repoMock.Setup(userRepo => userRepo.Remove(It.IsAny<Guid>()));

        var repo = repoMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;

        var itemId = Guid.NewGuid();

        var userMock = new Mock<UserFacade>(repo, mapper) { CallBase = true };
        userMock.Setup(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()));

        // Act
        userMock.Object.Delete(itemId, Guid.Parse("53171385-BFFD-4A2A-4661-08DD16E533FD").ToString());

        // Assert
        userMock.Verify(f => f.ThrowIfWrongOwner(It.IsAny<Guid>(), It.IsAny<string?>()), Times.Once);
        repoMock.Verify(userRepo => userRepo.Remove(itemId), Times.Once);
    }
}
