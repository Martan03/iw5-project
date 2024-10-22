using AutoMapper;
using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{    // přidat při merge s DAL
    public class QuestionFacade(/*IQuestionRepository*/ questionRepository, IMapper mapper) : IQuestionFacade
    {
        public List<QuestionListModel> GetAll() 
        {
        
        }

        public List<QuestionListModel> SearchByText(string text) 
        {
        
        }

        public List<QuestionListModel> SearchByDescription(string description) 
        {

        }

        public QuestionDetailModel? GetById(Guid id) 
        {
        
        }

        public Guid CreateOrUpdate(QuestionDetailModel questionModel) 
        {
        
        }

        public Guid Create(QuestionDetailModel questionModel) 
        {
        
        }

        public Guid? Update(QuestionDetailModel questionModel) 
        {
        
        }

        public void Delete(Guid id)
        {
        
        }
    }
}
