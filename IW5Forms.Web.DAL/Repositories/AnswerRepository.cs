using IW5Forms.Common.Models.Answer;

namespace IW5Forms.Web.DAL.Repositories;

public class AnswerRepository : RepositoryBase<AnswerListAndDetailModel>
{
    public override string TableName { get; } = "Answers";

    public AnswerRepository(LocalDb localDb)
        : base(localDb)
    {
    }
}
