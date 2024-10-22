using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{
    public interface IQuestionFacade : IAppFacade
    {
        List<QuestionListModel> GetAll();
        List<QuestionListModel> SearchByText(string text);
        List<QuestionListModel> SearchByDescription(string description);
        QuestionDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(QuestionDetailModel questionModel);
        Guid Create(QuestionDetailModel questionModel);
        Guid? Update(QuestionDetailModel questionModel);
        void Delete(Guid id);
    }
}
