using IW5Forms.Common.Models.Question;

namespace IW5Forms.Web.DAL.Repositories;

public class QuestionRepository : RepositoryBase<QuestionDetailModel>
{
    public override string TableName { get; } = "Questions";

    public QuestionRepository(LocalDb localDb)
        : base(localDb)
    {
    }
}
