﻿// <auto-generated />
using System;
using IW5Forms.API.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    [DbContext(typeof(FormsDbContext))]
    [Migration("20241020165101_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FormEntityUserEntity", b =>
                {
                    b.Property<Guid>("AvailableFormsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersWithAccessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AvailableFormsId", "UsersWithAccessId");

                    b.HasIndex("UsersWithAccessId");

                    b.ToTable("FormEntityUserEntity");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.AnswerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.FormEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BeginTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CompletedUsersId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Incognito")
                        .HasColumnType("bit");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("SingleTry")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.QuestionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("QuestionType")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FormEntityUserEntity", b =>
                {
                    b.HasOne("IW5Forms.Api.DAL.Common.Entities.FormEntity", null)
                        .WithMany()
                        .HasForeignKey("AvailableFormsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IW5Forms.Api.DAL.Common.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersWithAccessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.AnswerEntity", b =>
                {
                    b.HasOne("IW5Forms.Api.DAL.Common.Entities.QuestionEntity", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.FormEntity", b =>
                {
                    b.HasOne("IW5Forms.Api.DAL.Common.Entities.UserEntity", "Owner")
                        .WithMany("OwnedForms")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.QuestionEntity", b =>
                {
                    b.HasOne("IW5Forms.Api.DAL.Common.Entities.FormEntity", "Form")
                        .WithMany("Questions")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.FormEntity", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.QuestionEntity", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("IW5Forms.Api.DAL.Common.Entities.UserEntity", b =>
                {
                    b.Navigation("OwnedForms");
                });
#pragma warning restore 612, 618
        }
    }
}
