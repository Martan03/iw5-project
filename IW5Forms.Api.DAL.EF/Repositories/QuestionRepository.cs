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
    public class QuestionRepository : RepositoryBase<QuestionEntity>, IQuestionRepository
    {
        private readonly IMapper mapper;
        public QuestionRepository(FormsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.mapper = mapper;
        }

        public override QuestionEntity? GetById(Guid id)
        {
            return DbContext.Questions
                .Include(question => question.Answers)
                .SingleOrDefault(entity => entity.Id == id);
        }

        public override Guid? Update(QuestionEntity questionEntity)
        {
            DbContext.Questions.Update(questionEntity!);
            return questionEntity!.Id;

            // This didnt work, but lets keep it just in case
            //if (Exists(questionEntity.Id))
            //{
            //    var existingQuestion = DbContext.Questions.Find(questionEntity.Id);
            //    //.Include(entity => entity.Answers)
            //    //.SingleOrDefault(question => question.Id == questionEntity.Id);

            //    if (existingQuestion != null)
            //    {
            //        DbContext.Entry(existingQuestion).CurrentValues.SetValues(questionEntity);
            //        //mapper.Map<QuestionEntity, QuestionEntity>(questionEntity, existingQuestion);
            //        //existingQuestion.Description = questionEntity.Description;
            //        //existingQuestion.Answers = questionEntity.Answers;
            //        //existingQuestion.Form = questionEntity.Form;
            //        //existingQuestion.FormId = questionEntity.FormId;
            //        //existingQuestion.Options = questionEntity.Options;
            //        //existingQuestion.QuestionType = existingQuestion.QuestionType;
            //        //existingQuestion.Text = questionEntity.Text;

            //        //DbContext.Questions.Update(existingQuestion!);
            //        DbContext.SaveChanges();

            //    }
            //    else
            //    {
            //    }
            //    return existingQuestion!.Id;

            //}
            //else return null;
        }
    }
}
