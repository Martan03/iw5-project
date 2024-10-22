using AutoMapper;
using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Form;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.Facades
{    // přidat při merge s DAL
    public class QuestionFacade(IQuestionRepository questionRepository, IMapper mapper) : IQuestionFacade
    {
        public List<QuestionListModel> GetAll() 
        {
            return mapper.Map<List<QuestionListModel>>(questionRepository.GetAll());

        }

        public List<QuestionListModel> SearchByText(string text)
        {
            //dodelat
            return new List<QuestionListModel>();
        }

        public List<QuestionListModel> SearchByDescription(string description)
        {
            //dodelat
            return new List<QuestionListModel>();
        }

        public QuestionDetailModel? GetById(Guid id) 
        {
            var questionEntity = questionRepository.GetById(id);
            return mapper.Map<QuestionDetailModel>(questionEntity);
        }

        public Guid CreateOrUpdate(QuestionDetailModel questionModel) 
        {
            return questionRepository.Exists(questionModel.Id)
                ? Update(questionModel)!.Value
                : Create(questionModel);
        }

        public Guid Create(QuestionDetailModel questionModel) 
        {
            var questionEntity = mapper.Map<QuestionEntity>(questionModel);
            return questionRepository.Insert(questionEntity);
        }

        public Guid? Update(QuestionDetailModel questionModel) 
        {
            var questionEntity = mapper.Map<QuestionEntity>(questionModel);
            return questionRepository.Update(questionEntity);
        }

        public void Delete(Guid id)
        {
            questionRepository.Remove(id);
        }
    }
}
