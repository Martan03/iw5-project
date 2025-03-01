using AutoMapper;
using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IW5Forms.Web.BL.Facades;


public class FormFacade : FacadeBase<FormDetailModel, FormListModel>
{
    private readonly IFormApiClient apiClient;

    public FormFacade(
        IFormApiClient apiClient,
        FormRepository formRepo,
        IMapper mapper,
        IOptions<LocalDbOptions> localDbOptions)
        : base(formRepo, mapper, localDbOptions)
    {
        this.apiClient = apiClient;
    }

    public override async Task<List<FormListModel>> GetAllAsync()
    {
        var formsAll = await base.GetAllAsync();

        var formsFromApi = await apiClient.FormGetAsync();
        foreach (var formFromApi in formsFromApi)
        {
            if (formsAll.Any(r => r.Id == formFromApi.Id) is false)
            {
                formsAll.Add(formFromApi);
            }
        }

        return formsAll;
    }

    public async Task<List<FormListModel>> GetManagableAsync()
    {
        var formsAll = await base.GetAllAsync();

        var formsFromApi = await apiClient.ManagableAsync();
        foreach (var formFromApi in formsFromApi)
        {
            if (formsAll.Any(r => r.Id == formFromApi.Id) is false)
            {
                formsAll.Add(formFromApi);
            }
        }

        return formsAll;
    }

    public override async Task<FormDetailModel> GetByIdAsync(Guid id)
    {
        return await apiClient.FormGetAsync(id);
    }

    protected override async Task<Guid> SaveToApiAsync(FormDetailModel data)
    {
        return await apiClient.UpsertAsync(data);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await apiClient.FormDeleteAsync(id);
    }
}
