﻿// <auto-generated />
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Dashboard.Migrations
{
    [DbContext(typeof(DashboardContext))]
    [Migration("20180419211930_DashboardMigration")]
    partial class DashboardMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Dashboard.Models.Comment", b =>
                {
                    b.Property<int>("CommentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cmt");

                    b.Property<DateTime>("Created_at");

                    b.Property<int>("MessageID");

                    b.Property<DateTime>("Updated_at");

                    b.Property<int>("UserID");

                    b.HasKey("CommentID");

                    b.HasIndex("MessageID");

                    b.HasIndex("UserID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Dashboard.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_at");

                    b.Property<string>("Msg");

                    b.Property<int>("ReceiverID");

                    b.Property<DateTime>("Updated_at");

                    b.Property<int>("WriterID");

                    b.HasKey("MessageID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("WriterID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Dashboard.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_at");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<DateTime>("Updated_at");

                    b.Property<int>("UserLevel");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dashboard.Models.Comment", b =>
                {
                    b.HasOne("Dashboard.Models.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dashboard.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dashboard.Models.Message", b =>
                {
                    b.HasOne("Dashboard.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dashboard.Models.User", "Writer")
                        .WithMany()
                        .HasForeignKey("WriterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
