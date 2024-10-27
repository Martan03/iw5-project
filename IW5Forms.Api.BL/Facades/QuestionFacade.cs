using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Answer;
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
            QuestionEntity newQuestionEntity = new QuestionEntity()
            {
                Answers = new List<AnswerEntity>(),
                Description = questionModel.Description,
                Options = questionModel.Options,
                Id = questionModel.Id,
                QuestionType = questionModel.QuestionType,
                Text = questionModel.Text,
            };

            foreach (var item in questionModel.Answers)
            {
                newQuestionEntity.Answers.Add(new AnswerEntity()
                {
                    Id = item.Id,
                    Question = newQuestionEntity,
                    QuestionId = newQuestionEntity.Id,
                    ResponderId = item.ResponderId,
                    Text = item.Text,
                });

            }
            return questionRepository.Insert(newQuestionEntity);
        }

        public Guid? Update(QuestionDetailModel questionModel)
        {
            var questionEntity = questionRepository.GetById(questionModel.Id);
            if (questionEntity == null) return null;
            questionEntity.Description = questionModel.Description;
            questionEntity.Options = questionModel.Options;
            questionEntity.QuestionType = questionModel.QuestionType;
            questionEntity.Text = questionModel.Text;
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
