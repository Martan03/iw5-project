using AutoMapper;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IW5Forms.Web.BL.Facades;

public class AnswerFacade:
    FacadeBase<AnswerListAndDetailModel, AnswerListAndDetailModel>
{
    private readonly IAnswerApiClient apiClient;

    public AnswerFacade(
        IAnswerApiClient apiClient,
        AnswerRepository answerRepo,
        IMapper mapper,
        IOptions<LocalDbOptions> localDbOptions)
        : base(answerRepo, mapper, localDbOptions)
    {
        this.apiClient = apiClient;
    }

    public override async Task<List<AnswerListAndDetailModel>> GetAllAsync()
    {
        var answersAll = await base.GetAllAsync();

        var answersFromApi = await apiClient.AnswerGetAsync();
        foreach (var answerFromApi in answersFromApi)
        {
            if (answersAll.Any(r => r.Id == answerFromApi.Id) is false)
            {
                answersAll.Add(answerFromApi);
            }
        }

        return answersAll;
    }

    public override async Task<AnswerListAndDetailModel> GetByIdAsync(Guid id)
    {
        return await apiClient.AnswerGetAsync(id);
    }

    protected override async Task<Guid> SaveToApiAsync(
        AnswerListAndDetailModel data
    ) {
        return await apiClient.UpsertAsync(data);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await apiClient.AnswerDeleteAsync(id);
    }
}
