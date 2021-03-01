﻿// <auto-generated />
using System;
using Chartix.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chartix.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210130183758_UpdateSourceDateAndMetricCreated")]
    partial class UpdateSourceDateAndMetricCreated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Chartix.Core.Entities.Metric", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMain")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("SourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Metric");
                });

            modelBuilder.Entity("Chartix.Core.Entities.ProcessedUpdate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("ExternalId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UpdateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ProcessedUpdate");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Source", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("ExternalId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastActionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId");

                    b.ToTable("Source");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Value", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Content")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MetricId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ValueDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MetricId");

                    b.ToTable("Value");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Metric", b =>
                {
                    b.HasOne("Chartix.Core.Entities.Source", "Source")
                        .WithMany("Metrics")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Value", b =>
                {
                    b.HasOne("Chartix.Core.Entities.Metric", "Metric")
                        .WithMany("Values")
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metric");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Metric", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Chartix.Core.Entities.Source", b =>
                {
                    b.Navigation("Metrics");
                });
#pragma warning restore 612, 618
        }
    }
}
