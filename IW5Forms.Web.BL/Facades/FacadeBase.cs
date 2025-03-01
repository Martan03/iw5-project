using AutoMapper;
using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IW5Forms.Web.BL.Facades;

public abstract class FacadeBase<TDetailModel, TListModel> : IAppFacade
    where TDetailModel : IWithId
{
    private readonly RepositoryBase<TDetailModel> repository;
    private readonly IMapper mapper;
    private readonly LocalDbOptions localDbOptions;
    protected virtual string apiVersion => "1";

    protected FacadeBase(
        RepositoryBase<TDetailModel> repository,
        IMapper mapper,
        IOptions<LocalDbOptions> localDbOptions)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.localDbOptions = localDbOptions.Value;
    }

    public virtual async Task<List<TListModel>> GetAllAsync()
    {
        var itemsAll = new List<TListModel>();

        if (localDbOptions.IsLocalDbEnabled)
        {
            itemsAll.AddRange(await GetAllFromLocalDbAsync());
        }
        return itemsAll;
    }

    protected async Task<IList<TListModel>> GetAllFromLocalDbAsync()
    {
        var recipesLocal = await repository.GetAllAsync();
        return mapper.Map<IList<TListModel>>(recipesLocal);
    }

    public abstract Task<TDetailModel> GetByIdAsync(Guid id);

    public virtual async Task SaveAsync(TDetailModel data)
    {
        try
        {
            await SaveToApiAsync(data);
        }
        catch (HttpRequestException exception)
            when (exception.Message.Contains("Failed to fetch"))
        {
            if (localDbOptions.IsLocalDbEnabled)
            {
                await repository.InsertAsync(data);
            }
        }
    }

    protected abstract Task<Guid> SaveToApiAsync(TDetailModel data);
    public abstract Task DeleteAsync(Guid id);

    public async Task<bool> SynchronizeLocalDataAsync()
    {
        var localItems = await repository.GetAllAsync();
        foreach (var localItem in localItems)
        {
            await SaveToApiAsync(localItem);
            await repository.RemoveAsync(localItem.Id);
        }

        return localItems.Any();
    }
}
