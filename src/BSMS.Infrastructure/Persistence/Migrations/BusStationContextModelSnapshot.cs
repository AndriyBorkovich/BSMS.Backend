﻿// <auto-generated />
using System;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(BusStationContext))]
    partial class BusStationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BSMS.Core.Entities.Bus", b =>
                {
                    b.Property<int>("BusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BusId");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusReview", b =>
                {
                    b.Property<int>("BusReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusReviewId"));

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<int?>("ComfortRating")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InternetConnectionRating")
                        .HasColumnType("int");

                    b.Property<int>("PassengerId")
                        .HasColumnType("int");

                    b.Property<int?>("PriceQualityRatioRating")
                        .HasColumnType("int");

                    b.Property<int?>("PunctualityRating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SanitaryConditionsRating")
                        .HasColumnType("int");

                    b.HasKey("BusReviewId");

                    b.HasIndex("BusId");

                    b.HasIndex("PassengerId");

                    b.ToTable("BusReviews");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusScheduleEntry", b =>
                {
                    b.Property<int>("BusScheduleEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusScheduleEntryId"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MoveDirection")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("BusScheduleEntryId");

                    b.HasIndex("BusId");

                    b.HasIndex("RouteId");

                    b.ToTable("BusScheduleEntry");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DriverId"));

                    b.Property<int?>("BusId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("DriverLicense")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DriverId");

                    b.HasIndex("BusId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Passenger", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerId"));

                    b.Property<int?>("BusId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PassengerId");

                    b.HasIndex("BusId");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteId"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RouteId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeatId"));

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFree")
                        .HasColumnType("bit");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("int");

                    b.HasKey("SeatId");

                    b.HasIndex("BusId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Stop", b =>
                {
                    b.Property<int>("StopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StopId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("PreviousStopId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("StopId");

                    b.HasIndex("PreviousStopId");

                    b.HasIndex("RouteId");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"));

                    b.Property<int>("EndStopId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSold")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<int>("StartStopId")
                        .HasColumnType("int");

                    b.HasKey("TicketId");

                    b.HasIndex("EndStopId");

                    b.HasIndex("SeatId")
                        .IsUnique();

                    b.HasIndex("StartStopId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("BSMS.Core.Entities.TicketPayment", b =>
                {
                    b.Property<int>("TicketPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketPaymentId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PassengerId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.HasKey("TicketPaymentId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("TicketPayment");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripId"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("BusScheduleEntryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("TripId");

                    b.HasIndex("BusScheduleEntryId")
                        .IsUnique();

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusReview", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Bus", "Bus")
                        .WithMany("BusReviews")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Passenger", "Passenger")
                        .WithMany("BusReviews")
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusScheduleEntry", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Bus", "Bus")
                        .WithMany("BusScheduleEntries")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Route", "Route")
                        .WithMany("BusScheduleEntries")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Driver", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Bus", "Bus")
                        .WithMany("Drivers")
                        .HasForeignKey("BusId");

                    b.HasOne("BSMS.Core.Entities.Company", "Company")
                        .WithMany("Drivers")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Bus");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Passenger", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Bus", "Bus")
                        .WithMany("Passengers")
                        .HasForeignKey("BusId");

                    b.Navigation("Bus");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Seat", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Bus", "Bus")
                        .WithMany("Seats")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Stop", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Stop", "PreviousStop")
                        .WithMany()
                        .HasForeignKey("PreviousStopId");

                    b.HasOne("BSMS.Core.Entities.Route", "Route")
                        .WithMany("Stops")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PreviousStop");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Ticket", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Stop", "EndStop")
                        .WithMany("TicketEndStops")
                        .HasForeignKey("EndStopId")
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Seat", "Seat")
                        .WithOne("Ticket")
                        .HasForeignKey("BSMS.Core.Entities.Ticket", "SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Stop", "StartStop")
                        .WithMany("TicketStartStops")
                        .HasForeignKey("StartStopId")
                        .IsRequired();

                    b.Navigation("EndStop");

                    b.Navigation("Seat");

                    b.Navigation("StartStop");
                });

            modelBuilder.Entity("BSMS.Core.Entities.TicketPayment", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Passenger", "Passenger")
                        .WithMany("Payments")
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Ticket", "Ticket")
                        .WithOne("Payment")
                        .HasForeignKey("BSMS.Core.Entities.TicketPayment", "TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Trip", b =>
                {
                    b.HasOne("BSMS.Core.Entities.BusScheduleEntry", "BusScheduleEntry")
                        .WithOne("Trip")
                        .HasForeignKey("BSMS.Core.Entities.Trip", "BusScheduleEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusScheduleEntry");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Bus", b =>
                {
                    b.Navigation("BusReviews");

                    b.Navigation("BusScheduleEntries");

                    b.Navigation("Drivers");

                    b.Navigation("Passengers");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusScheduleEntry", b =>
                {
                    b.Navigation("Trip");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Company", b =>
                {
                    b.Navigation("Drivers");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Passenger", b =>
                {
                    b.Navigation("BusReviews");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Route", b =>
                {
                    b.Navigation("BusScheduleEntries");

                    b.Navigation("Stops");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Seat", b =>
                {
                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Stop", b =>
                {
                    b.Navigation("TicketEndStops");

                    b.Navigation("TicketStartStops");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Ticket", b =>
                {
                    b.Navigation("Payment")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
