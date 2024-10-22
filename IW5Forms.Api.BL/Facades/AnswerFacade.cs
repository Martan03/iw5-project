using AutoMapper;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.BL.Facades
{  
    public class AnswerFacade(IAnswerRepository answerRepository, IMapper mapper) : IAnswerFacade
    {
        public List<AnswerListAndDetailModel> GetAll()
        {
            return mapper.Map<List<AnswerListAndDetailModel>>(answerRepository.GetAll());
        }

        public AnswerListAndDetailModel? GetById(Guid id)
        {
            var answerEntity = answerRepository.GetById(id);
            return mapper.Map<AnswerListAndDetailModel>(answerEntity);
        }

        public Guid CreateOrUpdate(AnswerListAndDetailModel answerModel)
        {
            return answerRepository.Exists(answerModel.Id)
                ? Update(answerModel)!.Value
                : Create(answerModel);

        }

        public Guid Create(AnswerListAndDetailModel answerModel)
        {
            var answerEntity = mapper.Map<AnswerEntity>(answerModel);
            return answerRepository.Insert(answerEntity);
        }

        public Guid? Update(AnswerListAndDetailModel answerModel)
        {
            var answerEntity = mapper.Map<AnswerEntity>(answerModel);
            return answerRepository.Update(answerEntity);
        }

        public void Delete(Guid id)
        {
            answerRepository.Remove(id);
        }
    }
}
