using IW5Forms.Common.Models.User;

namespace IW5Forms.Web.DAL.Repositories;

public class UserRepository : RepositoryBase<UserDetailModel>
{
    public override string TableName { get; } = "Users";

    public UserRepository(LocalDb localDb)
        : base(localDb)
    {
    }
}
