using AutoMapper;
using IW5Forms.Common.Models.User;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IW5Forms.Web.BL.Facades;

public class UserFacade : FacadeBase<UserDetailModel, UserListModel>
{
    private readonly IUserApiClient apiClient;

    public UserFacade(
        IUserApiClient apiClient,
        UserRepository userRepo,
        IMapper mapper,
        IOptions<LocalDbOptions> localDbOptions)
        : base(userRepo, mapper, localDbOptions)
    {
        this.apiClient = apiClient;
    }

    public override async Task<List<UserListModel>> GetAllAsync()
    {
        var usersAll = await base.GetAllAsync();

        var usersFromApi = await apiClient.UserGetAsync();
        foreach (var userFromApi in usersFromApi)
        {
            if (usersAll.Any(r => r.Id == userFromApi.Id) is false)
            {
                usersAll.Add(userFromApi);
            }
        }

        return usersAll;
    }

    public override async Task<UserDetailModel> GetByIdAsync(Guid id)
    {
        return await apiClient.UserGetAsync(id);
    }

    protected override async Task<Guid> SaveToApiAsync(UserDetailModel data)
    {
        return await apiClient.UpsertAsync(data);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await apiClient.UserDeleteAsync(id);
    }
}
