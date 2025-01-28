using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace IW5Forms.Api.DAL.EF.Repositories
{
    public class FormRepository : RepositoryBase<FormEntity>, IFormRepository
    {
        private readonly IMapper mapper;
        public FormRepository(FormsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.mapper = mapper;
        }

        public override IList<FormEntity> GetAll()
        {
            return DbContext.Forms
                .Include(entity => entity.Questions)
                .ToList();

        }

        public override FormEntity? GetById(Guid id)
        {
            return DbContext.Forms
                .Include(entity => entity.Questions)
                .SingleOrDefault(entity => entity.Id == id);
        }

        public override Guid? Update(FormEntity formEntity)
        {
            if (Exists(formEntity.Id))
            {
                var existingForm = DbContext.Forms
                    .Include(entity => entity.Questions)
                    .SingleOrDefault(form => form.Id == formEntity.Id);

                existingForm.CompletedUsersId = formEntity.CompletedUsersId;
                existingForm.BeginTime = formEntity.BeginTime;
                existingForm.EndTime= formEntity.EndTime;
                existingForm.Incognito= formEntity.Incognito;
                existingForm.Name= formEntity.Name;
                existingForm.Owner= formEntity.Owner;
                existingForm.OwnerId= formEntity.OwnerId;
                existingForm.Questions= formEntity.Questions;
                existingForm.SingleTry= formEntity.SingleTry;

                DbContext.Forms.Update(existingForm!);
                DbContext.SaveChanges();

                return existingForm!.Id;
            }
            else return null;
        }
    }

}
