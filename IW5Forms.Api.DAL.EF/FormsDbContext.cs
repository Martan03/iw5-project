using Microsoft.EntityFrameworkCore;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.API.DAL
{
    public class FormsDbContext : DbContext
    {
        public DbSet<UserEntity>  Users { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }
        public DbSet<FormEntity> Forms { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }


        public FormsDbContext(DbContextOptions<FormsDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany(userEntity => userEntity.Forms)
                .WithMany(formEntity => formEntity.Users);
        }
    }
}
