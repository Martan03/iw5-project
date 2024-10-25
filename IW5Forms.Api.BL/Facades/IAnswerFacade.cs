using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{
    public interface IAnswerFacade : IAppFacade
    {
        List<AnswerListAndDetailModel> GetAll();
        AnswerListAndDetailModel? GetById(Guid id);
        //Guid CreateOrUpdate(AnswerListAndDetailModel answerModel);
        //Guid Create(AnswerListAndDetailModel answerModel);
        //Guid? Update(AnswerListAndDetailModel answerModel);
        void Delete(Guid id);
    }
}
