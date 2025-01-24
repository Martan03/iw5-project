using AutoMapper;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Question;

namespace IW5Forms.Api.BL.Facades
{
    public class AnswerFacade : FacadeBase<IAnswerRepository, AnswerEntity>, IAnswerFacade
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerFacade(
            IAnswerRepository answerRepository,
            IMapper mapper)
            : base(answerRepository)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public List<AnswerListAndDetailModel> GetAll()
        {
            return _mapper.Map<List<AnswerListAndDetailModel>>(_answerRepository.GetAll());
        }

        public AnswerListAndDetailModel? GetById(Guid id)
        {
            var answerEntity = _answerRepository.GetById(id);
            return _mapper.Map<AnswerListAndDetailModel>(answerEntity);
        }

        public Guid CreateOrUpdate(AnswerListAndDetailModel answerModel, string? ownerId)
        {
            return _answerRepository.Exists(answerModel.Id) ? Update(answerModel, ownerId)!.Value : Create(answerModel, ownerId);
        }

        public Guid Create(AnswerListAndDetailModel answerModel, string? ownerId)
        {
            AnswerEntity newAnswerEntity = new AnswerEntity()
            {
                Id = answerModel.Id,
                Text = answerModel.Text,
                IdentityOwnerId = ownerId,
            };

            return _answerRepository.Insert(newAnswerEntity);
        }

        public Guid? Update(AnswerListAndDetailModel answerModel, string? ownerId = null)
        {
            ThrowIfWrongOwner(answerModel.Id, ownerId);
            AnswerEntity? newAnswerEntity = _answerRepository.GetById(answerModel.Id);
            if (newAnswerEntity == null) return null;
            newAnswerEntity.IdentityOwnerId = answerModel.IdentityOwnerId;
            newAnswerEntity.Text = answerModel.Text;

            return _answerRepository.Update(newAnswerEntity);
        }

        public void Delete(Guid id, string? ownerId = null)
        {
            ThrowIfWrongOwner(id, ownerId);
            _answerRepository.Remove(id);
        }
    }
}
