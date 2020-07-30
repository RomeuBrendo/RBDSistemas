﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RBDNumeros.Infra.Repositories;

namespace RBDNumeros.Infra.Migrations
{
    [DbContext(typeof(RBDNumerosContext))]
    partial class RBDNumerosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Cliente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Carteira")
                        .HasColumnType("int")
                        .HasMaxLength(1);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.ConfiguracaoPlanilha", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Carteira")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("ClienteNome")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("DataAberturaTicket")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("DataResolvido")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("NumeroTicket")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.Property<string>("Tecnico")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ConfiguracaoPlanilha");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Sla", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<TimeSpan>("Acima20")
                        .HasColumnType("time(6)")
                        .HasMaxLength(10);

                    b.Property<TimeSpan>("Dentro")
                        .HasColumnType("time(6)")
                        .HasMaxLength(10);

                    b.Property<TimeSpan>("Estourado")
                        .HasColumnType("time(6)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Sla");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Tecnico", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Carteira")
                        .HasColumnType("int");

                    b.Property<bool>("ContabilizarNumeros")
                        .HasColumnName("ContabilizarNumeros")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Tecnico");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Carteira")
                        .HasColumnType("int");

                    b.Property<Guid?>("CategoriaId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ClienteId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DataAberturaTicket")
                        .HasColumnName("DataFinalizacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataResolvido")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("NumeroTicket")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("TecnicoId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("TecnicoId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("RBDNumeros.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("RBDNumeros.Domain.Entities.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId");

                    b.HasOne("RBDNumeros.Domain.Entities.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("RBDNumeros.Domain.Entities.Tecnico", "Tecnico")
                        .WithMany()
                        .HasForeignKey("TecnicoId");
                });
#pragma warning restore 612, 618
        }
    }
}
