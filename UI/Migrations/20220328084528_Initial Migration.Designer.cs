﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UI.Data;

#nullable disable

namespace UI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220328084528_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UI.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeeId");

                    b.HasIndex("RoleId");

                    b.HasIndex("PersonId", "Email")
                        .IsUnique();

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            Email = "superadmin@gmail.com",
                            Password = "admin",
                            PersonId = 1,
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("UI.Models.ListItem", b =>
                {
                    b.Property<int>("ListItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ListItemId"));

                    b.Property<int>("ListItemCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("ListItemName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ListItemId");

                    b.HasIndex("ListItemCategoryId");

                    b.HasIndex("ListItemName")
                        .IsUnique();

                    b.ToTable("listItems");

                    b.HasData(
                        new
                        {
                            ListItemId = 1,
                            ListItemCategoryId = 1,
                            ListItemName = "Male"
                        },
                        new
                        {
                            ListItemId = 2,
                            ListItemCategoryId = 1,
                            ListItemName = "Female"
                        });
                });

            modelBuilder.Entity("UI.Models.ListItemCategory", b =>
                {
                    b.Property<int>("ListItemCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ListItemCategoryId"));

                    b.Property<string>("ListItemCategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ListItemCategoryId");

                    b.ToTable("ListItemCategories");

                    b.HasData(
                        new
                        {
                            ListItemCategoryId = 1,
                            ListItemCategoryName = "Gender"
                        });
                });

            modelBuilder.Entity("UI.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PersonId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GenderListItemId")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            PersonId = 1,
                            FirstName = "Admin",
                            GenderListItemId = 1,
                            LastName = "Admin"
                        });
                });

            modelBuilder.Entity("UI.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "SuperAdmin"
                        });
                });

            modelBuilder.Entity("UI.Models.Employee", b =>
                {
                    b.HasOne("UI.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UI.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UI.Models.ListItem", b =>
                {
                    b.HasOne("UI.Models.ListItemCategory", "ListItemCategory")
                        .WithMany()
                        .HasForeignKey("ListItemCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListItemCategory");
                });
#pragma warning restore 612, 618
        }
    }
}