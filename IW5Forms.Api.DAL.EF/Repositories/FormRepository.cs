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

        public override Guid? Update(FormEntity questionEntity)
        {
            if (Exists(questionEntity.Id))
            {
                var existingForm = DbContext.Forms
                    .Include(entity => entity.Questions)
                    .SingleOrDefault(form => form.Id == questionEntity.Id);

                mapper.Map(questionEntity, existingForm);

                DbContext.Forms.Update(existingForm!);
                DbContext.SaveChanges();

                return existingForm!.Id;
            }
            else return null;
        }
    }

}
