using AutoMapper;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.Facades
{
    public class FormFacade : FacadeBase<IFormRepository, FormEntity>, IFormFacade
    {
        private readonly IFormRepository _formRepository;
        private readonly IMapper _mapper;

        public FormFacade(IFormRepository formRepository, IMapper mapper) : base(formRepository)
        {
            _formRepository = formRepository;
            _mapper = mapper;
        }

        public List<FormListModel> GetAll()
        {
            return _mapper.Map<List<FormListModel>>(_formRepository.GetAll());

        }

        public List<FormListModel> GetAllOwned(string? ownerId)
        {
            var allForms = _formRepository.GetAll().Where(form => form.IdentityOwnerId == ownerId).ToList();
            return _mapper.Map<List<FormListModel>>(allForms);
        }

        public FormDetailModel? GetById(Guid id)
        {
            var formEntity = _formRepository.GetById(id);
            return _mapper.Map<FormDetailModel>(formEntity);
        }

        public Guid CreateOrUpdate(FormDetailModel formModel, string? ownerId)
        {
            return _formRepository.Exists(formModel.Id) ? Update(formModel, ownerId)!.Value : Create(formModel, ownerId);
        }

        public Guid Create(FormDetailModel formModel, string? ownerId)
        {
            var newFormEntity = new FormEntity()
            {
                BeginTime = formModel.BeginTime,
                CompletedUsersId = formModel.CompletedUsersId,
                EndTime = formModel.EndTime,
                Id = formModel.Id,
                Incognito = formModel.Incognito,
                Name = formModel.Name,
                Questions = new List<QuestionEntity>(),
                SingleTry = formModel.SingleTry,
                IdentityOwnerId = ownerId

            };
            foreach (var question in formModel.Questions)
            {
                newFormEntity.Questions.Add(new QuestionEntity()
                { Answers = new List<AnswerEntity>(), Form = newFormEntity, FormId = newFormEntity.Id, Id = question.Id, Text = question.Text, QuestionType = question.QuestionType, Options = new List<string>() });
            }
            return _formRepository.Insert(newFormEntity);
        }

        public Guid? Update(FormDetailModel formModel, string? ownerId = null)
        {
            ThrowIfWrongOwner(formModel.Id, ownerId);
            var newFormEntity = _formRepository.GetById(formModel.Id);
            if (newFormEntity == null) return null;

            newFormEntity.BeginTime = formModel.BeginTime;
            newFormEntity.CompletedUsersId = formModel.CompletedUsersId;
            newFormEntity.EndTime = formModel.EndTime;
            newFormEntity.Incognito = formModel.Incognito;
            newFormEntity.Name = formModel.Name;
            newFormEntity.SingleTry = formModel.SingleTry;

            newFormEntity.Questions = new List<QuestionEntity>();
            foreach (var question in formModel.Questions)
            {
                newFormEntity.Questions.Add(new QuestionEntity()
                {
                    Id = question.Id,
                    QuestionType = question.QuestionType,
                    Text = question.Text,
                    Description = question.Description,
                    Options = question.Options,
                    Answers = new List<AnswerEntity>(),
                    Form = newFormEntity,
                    FormId = newFormEntity.Id,
                });
            }
            return _formRepository.Update(newFormEntity);
        }

        public void Delete(Guid id, string? ownerId = null)
        {
            ThrowIfWrongOwner(id, ownerId);
            _formRepository.Remove(id);
        }

    }
}
