﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SampleMassTransit.Infrastructure;

namespace SampleMassTransit.Infrastructure.Migrations
{
    [DbContext(typeof(OrderStateDbContext))]
    partial class OrderStateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SampleMassTransit.Components.OrderState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<string>("CurrentState")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CustomerNumber")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("SubmitDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("CorrelationId");

                    b.ToTable("OrderState");
                });
#pragma warning restore 612, 618
        }
    }
}
