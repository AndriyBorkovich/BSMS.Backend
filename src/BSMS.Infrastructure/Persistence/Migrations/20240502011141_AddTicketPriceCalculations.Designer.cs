﻿// <auto-generated />
using System;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(BusStationContext))]
    [Migration("20240502011141_AddTicketPriceCalculations")]
    partial class AddTicketPriceCalculations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("BusId");

                    b.HasIndex("DriverId");

                    b.HasIndex("Number");

                    b.ToTable("Buses");

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusReview", b =>
                {
                    b.Property<int>("BusReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusReviewId"));

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<int>("ComfortRating")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("InternetConnectionRating")
                        .HasColumnType("int");

                    b.Property<int>("PassengerId")
                        .HasColumnType("int");

                    b.Property<int>("PriceQualityRatioRating")
                        .HasColumnType("int");

                    b.Property<int>("PunctualityRating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SanitaryConditionsRating")
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

                    b.Property<TimeOnly>("ArrivalTime")
                        .HasColumnType("time");

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<TimeOnly>("DepartureTime")
                        .HasColumnType("time");

                    b.Property<string>("MoveDirection")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("BusScheduleEntryId");

                    b.HasIndex("BusId");

                    b.HasIndex("Day");

                    b.HasIndex("RouteId");

                    b.ToTable("BusScheduleEntries");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ContactPhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DriverId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("DriverLicense")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DriverId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Passenger", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerId"));

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PassengerId");

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

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
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OverallDistance")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("dbo.CalculateTotalDistanceForRoute([RouteId])");

                    b.HasKey("RouteId");

                    b.ToTable("Routes");

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
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

                    b.Property<int?>("DistanceToPrevious")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PreviousStopId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("StopId");

                    b.HasIndex("PreviousStopId");

                    b.HasIndex("RouteId");

                    b.ToTable("Stops");

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
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
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("decimal(18,2)")
                        .HasComputedColumnSql("dbo.CalculateTicketPrice([EndStopId])");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<int>("StartStopId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TicketId");

                    b.HasIndex("EndStopId");

                    b.HasIndex("SeatId");

                    b.HasIndex("StartStopId");

                    b.ToTable("Tickets");

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("BSMS.Core.Entities.TicketPayment", b =>
                {
                    b.Property<int>("TicketPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketPaymentId"));

                    b.Property<int>("PassengerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("TicketPaymentId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.HasIndex("TripId");

                    b.ToTable("TicketPayments");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripId"));

                    b.Property<DateTime?>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("BusScheduleEntryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TripId");

                    b.HasIndex("BusScheduleEntryId");

                    b.HasIndex("Status");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("BSMS.Core.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BSMS.Core.Views.BusDetailsView", b =>
                {
                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DriverName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("BusId");

                    b.ToTable((string)null);

                    b.ToView("BusDetailsView", (string)null);
                });

            modelBuilder.Entity("BSMS.Core.Entities.Bus", b =>
                {
                    b.HasOne("BSMS.Core.Entities.Driver", "Driver")
                        .WithMany("Buses")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");
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
                    b.HasOne("BSMS.Core.Entities.Company", "Company")
                        .WithMany("Drivers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
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
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId")
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
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("BSMS.Core.Entities.Trip", "Trip")
                        .WithMany("BoughtTickets")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");

                    b.Navigation("Ticket");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Trip", b =>
                {
                    b.HasOne("BSMS.Core.Entities.BusScheduleEntry", "BusScheduleEntry")
                        .WithMany("Trips")
                        .HasForeignKey("BusScheduleEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusScheduleEntry");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Bus", b =>
                {
                    b.Navigation("BusReviews");

                    b.Navigation("BusScheduleEntries");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("BSMS.Core.Entities.BusScheduleEntry", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Company", b =>
                {
                    b.Navigation("Drivers");
                });

            modelBuilder.Entity("BSMS.Core.Entities.Driver", b =>
                {
                    b.Navigation("Buses");
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
                    b.Navigation("Tickets");
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

            modelBuilder.Entity("BSMS.Core.Entities.Trip", b =>
                {
                    b.Navigation("BoughtTickets");
                });
#pragma warning restore 612, 618
        }
    }
}
