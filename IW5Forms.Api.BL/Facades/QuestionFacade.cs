using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Question;

namespace IW5Forms.Api.BL.Facades
{
    public class QuestionFacade : FacadeBase<IQuestionRepository, QuestionEntity>, IQuestionFacade
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionFacade(IQuestionRepository questionRepository, IMapper mapper) : base(questionRepository)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public List<QuestionListModel> GetAll()
        {
            return _mapper.Map<List<QuestionListModel>>(_questionRepository.GetAll());

        }

        public List<QuestionListModel> SearchByText(string text)
        {
            var questions = _mapper.Map<List<QuestionListModel>>(_questionRepository.GetAll());
            questions.RemoveAll(q => !q.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
            return questions;
        }


        public List<QuestionListModel> SearchByDescription(string description)
        {
            var questions = _mapper.Map<List<QuestionListModel>>(_questionRepository.GetAll());
            RemoveQuestionsNotContainingDescription(questions, description);
            return questions;
        }

        private void RemoveQuestionsNotContainingDescription(List<QuestionListModel> questions, string description)
        {
            foreach (var question in questions)
            {
                var questionDetail = _questionRepository.GetById(question.Id);

                if (questionDetail is null || questionDetail.Description is null || !questionDetail.Description.Contains(description, StringComparison.OrdinalIgnoreCase))
                {
                    questions.Remove(question);
                }
            }
        }

        public QuestionDetailModel? GetById(Guid id)
        {
            var questionEntity = _questionRepository.GetById(id);
            return _mapper.Map<QuestionDetailModel>(questionEntity);
        }

        public Guid CreateOrUpdate(QuestionDetailModel questionModel, string? ownerId)
        {
            if (_questionRepository.Exists(questionModel.Id))
                return Update(questionModel, ownerId)!.Value;
            return Create(questionModel, ownerId);
        }

        public Guid Create(QuestionDetailModel questionModel, string? ownerId)
        {
            QuestionEntity newQuestionEntity = new QuestionEntity()
            {
                Answers = new List<AnswerEntity>(),
                Description = questionModel.Description,
                Options = questionModel.Options,
                Id = questionModel.Id,
                QuestionType = questionModel.QuestionType,
                Text = questionModel.Text,
                IdentityOwnerId = ownerId,
                FormId = questionModel.FormId,
            };

            foreach (var item in questionModel.Answers)
            {
                newQuestionEntity.Answers.Add(new AnswerEntity()
                {
                    Id = item.Id,
                    Question = newQuestionEntity,
                    QuestionId = newQuestionEntity.Id,
                    Text = item.Text,
                    IdentityOwnerId = ownerId,
                });

            }
            return _questionRepository.Insert(newQuestionEntity);
        }

        public Guid? Update(QuestionDetailModel questionModel, string? ownerId = null)
        {
            ThrowIfWrongOwner(questionModel.Id, ownerId);
            var questionEntity = _questionRepository.GetById(questionModel.Id);
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
            return _questionRepository.Update(questionEntity);
        }

        public void Delete(Guid id, string? ownerId = null)
        {
            ThrowIfWrongOwner(id, ownerId);
            _questionRepository.Remove(id);
        }


    }
}
