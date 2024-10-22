using Microsoft.EntityFrameworkCore;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.API.DAL
{
    public class FormsDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }
        public DbSet<FormEntity> Forms { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<UserFormEntity> UserForms { get; set; }


        public FormsDbContext(DbContextOptions<FormsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1-N Form -> Question
            modelBuilder.Entity<FormEntity>()
                .HasMany(formEntity => formEntity.Questions)
                .WithOne(questionEntity => questionEntity.Form)
                .HasForeignKey(questionEntity => questionEntity.FormId);

            // 1-N Question -> Answer
            modelBuilder.Entity<QuestionEntity>()
                .HasMany(questionEntity => questionEntity.Answers)
                .WithOne(answerEntity => answerEntity.Question)
                .HasForeignKey(answerEntity => answerEntity.QuestionId);

            // 1-N User -> UserFormEntity
            modelBuilder.Entity<UserEntity>()
                .HasMany(userEntity => userEntity.Forms)
                .WithOne(formEntity => formEntity.User)
                .HasForeignKey(formEntity => formEntity.UserId);

        }
    }
}
