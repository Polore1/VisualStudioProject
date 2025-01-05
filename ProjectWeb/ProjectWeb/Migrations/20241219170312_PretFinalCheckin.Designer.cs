﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectWeb.Data;

#nullable disable

namespace ProjectWeb.Migrations
{
    [DbContext(typeof(ApplicationDB))]
    [Migration("20241219170312_PretFinalCheckin")]
    partial class PretFinalCheckin
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProjectWeb.Models.Entities.Checkin", b =>
                {
                    b.Property<int>("IdCheckin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdCheckin"));

                    b.Property<DateTime>("DataCheckin")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("GreutateBagaj")
                        .HasColumnType("decimal(5,2)");

                    b.Property<int>("IdUtilizator")
                        .HasColumnType("int");

                    b.Property<int>("IdZbor")
                        .HasColumnType("int");

                    b.Property<string>("LocRezervat")
                        .HasColumnType("longtext");

                    b.Property<decimal>("PretFinal")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("IdCheckin");

                    b.HasIndex("IdUtilizator");

                    b.HasIndex("IdZbor");

                    b.ToTable("Checkin");
                });

            modelBuilder.Entity("ProjectWeb.Models.Entities.Utilizator", b =>
                {
                    b.Property<int>("IdUtilizator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdUtilizator"));

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<int?>("IdZbor")
                        .HasColumnType("int");

                    b.Property<string>("Nume")
                        .HasColumnType("longtext");

                    b.Property<string>("Parola")
                        .HasColumnType("longtext");

                    b.Property<bool>("Pasager")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("IdUtilizator");

                    b.HasIndex("IdZbor");

                    b.ToTable("Utilizator");
                });

            modelBuilder.Entity("ProjectWeb.Models.Entities.Zbor", b =>
                {
                    b.Property<int>("IdZbor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdZbor"));

                    b.Property<DateTime?>("DataPlecare")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Destinatie")
                        .HasColumnType("longtext");

                    b.Property<decimal>("GreutateMaximaBagaj")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("Imbarcare")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LocuriDisponibile")
                        .HasColumnType("int");

                    b.Property<string>("NumeCompanie")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Pret")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<decimal>("TaxaSuplimentara")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("IdZbor");

                    b.ToTable("Zbor");
                });

            modelBuilder.Entity("ProjectWeb.Models.Entities.Checkin", b =>
                {
                    b.HasOne("ProjectWeb.Models.Entities.Utilizator", "Utilizator")
                        .WithMany()
                        .HasForeignKey("IdUtilizator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectWeb.Models.Entities.Zbor", "Zbor")
                        .WithMany()
                        .HasForeignKey("IdZbor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utilizator");

                    b.Navigation("Zbor");
                });

            modelBuilder.Entity("ProjectWeb.Models.Entities.Utilizator", b =>
                {
                    b.HasOne("ProjectWeb.Models.Entities.Zbor", "Zbor")
                        .WithMany()
                        .HasForeignKey("IdZbor");

                    b.Navigation("Zbor");
                });
#pragma warning restore 612, 618
        }
    }
}
