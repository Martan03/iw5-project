using AutoMapper;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.BL.Facades
{    // přidat při merge s DAL
    public class FormFacade(IFormRepository formRepository, IMapper mapper) : IFormFacade
    {
        public List<FormListModel> GetAll()
        {

        }

        public FormDetailModel? GetById(Guid id)
        {

        }

        public Guid CreateOrUpdate(FormDetailModel formModel)
        {

        }

        public Guid Create(FormDetailModel formModel) 
        {

        }

        public Guid? Update(FormDetailModel formModel)
        {

        }

        public void Delete(Guid id)
        {
        
        }

    }
}
