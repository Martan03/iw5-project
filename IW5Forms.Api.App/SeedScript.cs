using IW5Forms.Api.DAL.EF;
using IW5Forms.Api.DAL.Common;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Common.Models;

namespace IW5Forms.Api.App
{
    public class SeedScript
    {
        private readonly FormsDbContext dbContext;

        public SeedScript(FormsDbContext context)
        {
            dbContext = context;
        }

        public void SeedData()
        {
            UserEntity user = new UserEntity()
            {
                Id = Guid.NewGuid(), Name = "Fabio", Role = UserRoles.User,
                OwnedForms = new List<FormEntity>()
                {
                    new FormEntity()
                    {
                        BeginTime = DateTime.Today, EndTime = DateTime.Now, Id = Guid.NewGuid(), Incognito = false,
                        Name = "Lets goo", Public = true, OwnerId = Guid.NewGuid(),
                        SingleTry = false, Questions = new List<QuestionEntity>()
                        {
                            new QuestionEntity()
                            {
                                Description = "random desc", Id = Guid.NewGuid(), QuestionType = QuestionType.Text,
                                Text = "Kolik je hodin?"
                            },
                            new QuestionEntity()
                            {
                                Description = "random desc 2", Id = Guid.NewGuid(), QuestionType = QuestionType.Text,
                                Text = "Kolik je času?"
                            },
                            new QuestionEntity()
                            {
                                Description = "random desc 3", Id = Guid.NewGuid(), QuestionType = QuestionType.Text,
                                Text = "Kolik je minut?",
                                Answers = new List<AnswerEntity>()
                                {
                                    new AnswerEntity(){Id = Guid.NewGuid(), Text = "34"}
                                }
                            }

                        }
                    }
                }

            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
    }
}
