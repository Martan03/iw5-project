using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.DAL.IntegrationTests;

public interface IDatabaseFixture
{
    AnswerEntity? GetAnswerDirectly(Guid answerId);
    QuestionEntity? GetQuestionDirectly(Guid questionId);
    FormEntity? GetFormDirectly(Guid formId);
    IQuestionRepository GetQuestionRepository();
    IFormRepository GetFormRepository();
    IList<Guid> AnswerGuids { get; }
    IList<Guid> QuestionGuids { get; }
    IList<Guid> FormGuids { get; }
    IList<Guid> UserGuids { get; }
}
