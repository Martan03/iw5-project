using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{
    public interface IFormFacade : IAppFacade
    {
        List<FormListModel> GetAll();
        FormDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(FormDetailModel formModel, string? ownerId);
        Guid Create(FormDetailModel formModel, string? ownerId);
        Guid? Update(FormDetailModel formModel, string? ownerId);
        void Delete(Guid id, string? ownerId);
    }
}