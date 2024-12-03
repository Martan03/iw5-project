using AutoMapper;
using IW5Forms.Common.Models.Question;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IW5Forms.Web.BL.Facades;

public class QuestionFacade:
    FacadeBase<QuestionDetailModel, QuestionListModel>
{
    private readonly IQuestionApiClient apiClient;

    public QuestionFacade(
        IQuestionApiClient apiClient,
        QuestionRepository ingredientRepo,
        IMapper mapper,
        IOptions<LocalDbOptions> localDbOptions)
        : base(ingredientRepo, mapper, localDbOptions)
    {
        this.apiClient = apiClient;
    }

    public override async Task<List<QuestionListModel>> GetAllAsync()
    {
        var ingredientsAll = await base.GetAllAsync();

        var ingredientsFromApi = await apiClient.QuestionGetAsync();
        ingredientsAll.AddRange(ingredientsFromApi);

        return ingredientsAll;
    }

    public override async Task<QuestionDetailModel> GetByIdAsync(Guid id)
    {
        return await apiClient.QuestionGetAsync(id);
    }

    protected override async Task<Guid> SaveToApiAsync(
        QuestionDetailModel data
    ) {
        return await apiClient.UpsertAsync(data);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await apiClient.QuestionDeleteAsync(id);
    }
}
