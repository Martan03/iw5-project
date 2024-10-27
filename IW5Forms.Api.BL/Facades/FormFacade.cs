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
    public class FormFacade(IFormRepository formRepository, IMapper mapper) : IFormFacade
    {
        public List<FormListModel> GetAll()
        {
            return mapper.Map<List<FormListModel>>(formRepository.GetAll());

        }
        public FormDetailModel? GetById(Guid id)
        {
            var formEntity = formRepository.GetById(id);
            return mapper.Map<FormDetailModel>(formEntity);
        }

        public Guid CreateOrUpdate(FormDetailModel formModel)
        {
            return formRepository.Exists(formModel.Id)
                ? Update(formModel)!.Value
                : Create(formModel);
        }

        public Guid Create(FormDetailModel formModel)
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
                SingleTry = formModel.SingleTry

            };
            foreach (var question in formModel.Questions)
            {
                newFormEntity.Questions.Add(new QuestionEntity()
                { Answers = new List<AnswerEntity>(), Form = newFormEntity, FormId = newFormEntity.Id, Id = question.Id, Text = question.Text, QuestionType = question.QuestionType, Options = new List<string>() });
            }
            return formRepository.Insert(newFormEntity);
        }

        public Guid? Update(FormDetailModel formModel)
        {
            var newFormEntity = formRepository.GetById(formModel.Id);
            if (newFormEntity == null) return null;
            newFormEntity.BeginTime = formModel.BeginTime;
            newFormEntity.CompletedUsersId = formModel.CompletedUsersId;
            newFormEntity.EndTime = formModel.EndTime;
            newFormEntity.Incognito = formModel.Incognito;
            newFormEntity.Name = formModel.Name;

            newFormEntity.Questions = new List<QuestionEntity>();
            newFormEntity.SingleTry = formModel.SingleTry;



            foreach (var question in formModel.Questions)
            {
                newFormEntity.Questions.Add(new QuestionEntity()
                { Answers = new List<AnswerEntity>(), Form = newFormEntity, FormId = newFormEntity.Id, Id = question.Id, Text = question.Text, QuestionType = question.QuestionType, Options = new List<string>() });
            }
            return formRepository.Update(newFormEntity);
        }

        public void Delete(Guid id)
        {
            formRepository.Remove(id);
        }

    }
}
