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
            var formEntity = mapper.Map<FormEntity>(formModel);
            return formRepository.Insert(formEntity);
        }

        public Guid? Update(FormDetailModel formModel)
        {
            var formEntity = mapper.Map<FormEntity>(formModel);
            return formRepository.Update(formEntity);
        }

        public void Delete(Guid id)
        {
            formRepository.Remove(id);
        }

    }
}
