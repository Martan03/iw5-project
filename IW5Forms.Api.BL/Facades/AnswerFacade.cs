using AutoMapper;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.BL.Facades
{   // přidat při merge s DAL
    public class AnswerFacade(IAnswerRepository answerRepository, IMapper mapper) : IAnswerFacade
    {
        public List<AnswerListAndDetailModel> GetAll()
        {
            
        }

        public AnswerListAndDetailModel? GetById(Guid id)
        {
        
        }

        public Guid CreateOrUpdate(AnswerListAndDetailModel answerModel)
        {
        
        }

        public Guid Create(AnswerListAndDetailModel answerModel)
        {
        
        }

        public Guid? Update(AnswerListAndDetailModel answerModel)
        {

        }

        public void Delete(Guid id)
        {
        
        }
    }
}
