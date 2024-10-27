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
                .OnDelete(DeleteBehavior.Cascade);

            // 1-N User -> Answer
            modelBuilder.Entity<UserEntity>()
               .HasMany(typeof(AnswerEntity))
               .WithOne(nameof(AnswerEntity.Responder));

            // 1-N Question -> Answer
            modelBuilder.Entity<QuestionEntity>()
                .HasMany(questionEntity => questionEntity.Answers)
                .WithOne(answerEntity => answerEntity.Question)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-N User -> UserFormEntity
            modelBuilder.Entity<UserEntity>()
                .HasMany(userEntity => userEntity.Forms)
                .WithOne(userFormEntity => userFormEntity.User);

            // 1-N Form -> UserForm
            modelBuilder.Entity<FormEntity>()
                .HasMany(typeof(UserFormEntity))
                .WithOne(nameof(UserFormEntity.Form));
        }
    }
}
