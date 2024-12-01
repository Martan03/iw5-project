using IW5Forms.Common.Models.Form;

namespace IW5Forms.Web.DAL.Repositories;

public class FormRepository : RepositoryBase<FormDetailModel>
{
    public override string TableName { get; } = "Forms";

    public FormRepository(LocalDb localDb)
        : base(localDb)
    {
    }
}
