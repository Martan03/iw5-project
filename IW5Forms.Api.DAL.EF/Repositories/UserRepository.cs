﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IW5Forms.Api.DAL.EF.Repositories
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        private readonly IMapper mapper;
        public UserRepository(FormsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.mapper=mapper;
        }

        public override UserEntity? GetById(Guid id)
        {
            return DbContext.Users
                .Include(user => user.OwnedForms)
                .ThenInclude(form => form.Name)
                .Include(user => user.AvailableForms)
                .ThenInclude(form => form.Name)
                .SingleOrDefault(user => user.Id == id);
        }

        public void RemoveWIthOwnedForms(Guid id)
        {
            DbContext.Forms.Where(entity => entity.OwnerId == id).ExecuteDelete();
            base.Remove(id);
        }

        public override Guid? Update(UserEntity questionEntity)
        {
            if (Exists(questionEntity.Id))
            {
                var existingUser = DbContext.Users
                    .Include(entity => entity.AvailableForms)
                    .Include(entity => entity.OwnedForms)
                    .SingleOrDefault(user => user.Id == questionEntity.Id);

                mapper.Map(questionEntity, existingUser);

                DbContext.Users.Update(existingUser!);
                DbContext.SaveChanges();

                return existingUser!.Id;
            }
            else return null;
        }
    }
    
}
