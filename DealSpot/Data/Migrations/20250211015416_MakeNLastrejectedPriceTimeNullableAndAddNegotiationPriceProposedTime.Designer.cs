﻿// <auto-generated />
using System;
using DealSpot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DealSpot.Data.migrations
{
    [DbContext(typeof(DealSpotDBContext))]
    [Migration("20250211015416_MakeNLastrejectedPriceTimeNullableAndAddNegotiationPriceProposedTime")]
    partial class MakeNLastrejectedPriceTimeNullableAndAddNegotiationPriceProposedTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("DealSpot.Models.Negotiation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ActualPrice")
                        .HasColumnType("TEXT");

                    b.Property<int>("AttemptCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastRejectedPriceTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NegotiationPriceProposedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NegotiationStartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ProposedPrice")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("Negotiation");
                });

            modelBuilder.Entity("DealSpot.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("DealSpot.Models.Negotiation", b =>
                {
                    b.HasOne("DealSpot.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
