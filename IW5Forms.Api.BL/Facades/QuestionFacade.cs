using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Question;

namespace IW5Forms.Api.BL.Facades
{
    public class QuestionFacade(IQuestionRepository questionRepository, IMapper mapper) : IQuestionFacade
    {
        public List<QuestionListModel> GetAll() 
        {
            return mapper.Map<List<QuestionListModel>>(questionRepository.GetAll());

        }

        public List<QuestionListModel> SearchByText(string text)
        {
            var questions = mapper.Map<List<QuestionListModel>>(questionRepository.GetAll());
            questions.RemoveAll(q => !q.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
            return questions;
        }


        public List<QuestionListModel> SearchByDescription(string description)
        {
            var questions = mapper.Map<List<QuestionListModel>>(questionRepository.GetAll());
            RemoveQuestionsNotContainingDescription(questions, description);
            return questions;
        }

        private void RemoveQuestionsNotContainingDescription(List<QuestionListModel> questions, string description)
        {
            foreach (var question in questions)
            {
                var questionDetail = questionRepository.GetById(question.Id);

                if (questionDetail is null || questionDetail.Description is null || !questionDetail.Description.Contains(description, StringComparison.OrdinalIgnoreCase))
                {
                    questions.Remove(question);
                }
            }
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
            questionEntity.Answers = questionEntity.Answers.Select(t =>
                new AnswerEntity
                {
                    Id = t.Id,
                    Question = questionEntity,
                    QuestionId = questionEntity.Id,
                    ResponderId = t.ResponderId,
                    Text = t.Text
                }).ToList();
            return questionRepository.Update(questionEntity);
        }

        public void Delete(Guid id)
        {
            questionRepository.Remove(id);
        }
    }
}
