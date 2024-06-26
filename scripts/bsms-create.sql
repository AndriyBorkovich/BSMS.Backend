USE [master]
GO
/****** Object:  Database [BusStationDB]    Script Date: 10.05.2024 7:22:19 ******/
CREATE DATABASE [BusStationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BusStationDB', FILENAME = N'E:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BusStationDB.mdf' , SIZE = 270336KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BusStationDB_log', FILENAME = N'E:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BusStationDB_log.ldf' , SIZE = 1253376KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BusStationDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BusStationDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BusStationDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BusStationDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BusStationDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BusStationDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BusStationDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BusStationDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BusStationDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BusStationDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BusStationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BusStationDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BusStationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BusStationDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BusStationDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BusStationDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BusStationDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BusStationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BusStationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BusStationDB] SET TRUSTWORTHY ON 
GO
ALTER DATABASE [BusStationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BusStationDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BusStationDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BusStationDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BusStationDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BusStationDB] SET  MULTI_USER 
GO
ALTER DATABASE [BusStationDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BusStationDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BusStationDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BusStationDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BusStationDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BusStationDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BusStationDB', N'ON'
GO
ALTER DATABASE [BusStationDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [BusStationDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BusStationDB]
GO
/****** Object:  User [Pedro]    Script Date: 10.05.2024 7:22:19 ******/
CREATE USER [Pedro] FOR LOGIN [PedroLogin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [AndriiAdmin]    Script Date: 10.05.2024 7:22:19 ******/
CREATE USER [AndriiAdmin] FOR LOGIN [AndriiAdminLogin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [Passenger]    Script Date: 10.05.2024 7:22:19 ******/
CREATE ROLE [Passenger]
GO
/****** Object:  DatabaseRole [Driver]    Script Date: 10.05.2024 7:22:19 ******/
CREATE ROLE [Driver]
GO
ALTER ROLE [Passenger] ADD MEMBER [Pedro]
GO
ALTER ROLE [db_owner] ADD MEMBER [AndriiAdmin]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateNewTicketPrice]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

            CREATE   FUNCTION [dbo].[CalculateNewTicketPrice]
            (@StartStopId INT, @EndStopId INT)
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                    DECLARE @Distance1 INT, @Distance2 INT;
                    SELECT @Distance1 = DistanceToPrevious FROM Stops Where StopId = @StartStopId;
					SELECT @Distance2 = DistanceToPrevious FROM Stops Where StopId = @EndStopId;

                    RETURN ABS(ISNULL(@Distance2, 1) - ISNULL(@Distance1, 0)) * 0.3
                END
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateTicketPrice]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE   FUNCTION [dbo].[CalculateTicketPrice](@StopId INT)
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                    DECLARE  @RouteId INT;
                    SELECT @RouteId = RouteId FROM Stops Where StopId = @StopId;

                    RETURN dbo.CalculateTotalDistanceForRoute(@RouteId) * 0.3
                END
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateTotalDistanceForRoute]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[CalculateTotalDistanceForRoute]
                (@RouteId INT)
                RETURNS INT
                AS
                BEGIN
                    DECLARE @OverallDistance INT = 0;

                    SELECT @OverallDistance = MAX(ISNULL(DistanceToPrevious, 0))
                    FROM Stops
                    WHERE RouteId = @RouteId;

                RETURN @OverallDistance;
                END;
GO
/****** Object:  UserDefinedFunction [dbo].[FindNextStop]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE   FUNCTION [dbo].[FindNextStop]
                (@RouteId INT, @StopId INT)
                RETURNS INT
                AS
                BEGIN
                    DECLARE @NextStopId INT;

                    WITH RecursiveStops AS (
                        SELECT
                            StopId,
                            PreviousStopId
                        FROM
                            Stops
                        WHERE
                            RouteId = @RouteId
                            AND PreviousStopId IS NOT NULL
                    )

                    SELECT TOP 1
                        @NextStopId = StopId
                    FROM
                        RecursiveStops
                    WHERE
                        PreviousStopId = @StopId;

                    RETURN @NextStopId;
                END;
GO
/****** Object:  UserDefinedFunction [dbo].[SaveTableAsXml]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SaveTableAsXml](
@tableName NVARCHAR(30))
RETURNS NVARCHAR(MAX) -- xml converted to string
AS
BEGIN
	DECLARE @xmlResult XML
	DECLARE @sqlQuery NVARCHAR(200);
	SET @tableName = 'BusDetailsView';
	SET @sqlQuery = 'SELECT * FROM ' + @tableName + ' FOR XML AUTO, ROOT(''' + @tableName + ''')';

	-- Execute dynamic SQL and store result in @xmlResult variable
	EXEC sp_executesql @sqlQuery, N'@xmlResult XML OUTPUT', @xmlResult OUTPUT;

	DECLARE @xmlString NVARCHAR(MAX)
	SET @xmlString = CAST(@xmlResult AS NVARCHAR(MAX))
	RETURN @xmlString;
END
GO
/****** Object:  UserDefinedFunction [dbo].[StopsBelongToSameRoute]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE FUNCTION [dbo].[StopsBelongToSameRoute]
                (
                    @StopId1 INT,
                    @StopId2 INT
                )
                RETURNS BIT
                AS
                BEGIN
                    DECLARE @RouteId1 INT;
                    DECLARE @RouteId2 INT;

                    SELECT @RouteId1 = RouteId
                    FROM Stops
                    WHERE StopId = @StopId1;

                    SELECT @RouteId2 = RouteId
                    FROM Stops
                    WHERE StopId = @StopId2;

                    -- Порівняння RouteId
                    IF @RouteId1 = @RouteId2
                    BEGIN
                        RETURN 1;
                    END
                    
                    RETURN 0;
                END;
GO
/****** Object:  Table [dbo].[BusReviews]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusReviews](
	[BusReviewId] [int] IDENTITY(1,1) NOT NULL,
	[BusId] [int] NOT NULL,
	[PassengerId] [int] NOT NULL,
	[ComfortRating] [int] NOT NULL,
	[PunctualityRating] [int] NOT NULL,
	[PriceQualityRatioRating] [int] NOT NULL,
	[InternetConnectionRating] [int] NOT NULL,
	[SanitaryConditionsRating] [int] NOT NULL,
	[Comments] [nvarchar](200) NULL,
	[ReviewDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BusReviews] PRIMARY KEY CLUSTERED 
(
	[BusReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateBusRating]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[CalculateBusRating](@BusId INT)
                RETURNS FLOAT
				WITH SCHEMABINDING
                AS
                BEGIN
                    DECLARE @Rating FLOAT;
	                DECLARE @Count FLOAT = 5.0;
                    SELECT @Rating = AVG(ComfortRating + PunctualityRating + PriceQualityRatioRating + 
						                InternetConnectionRating + SanitaryConditionsRating) / @Count
                    FROM dbo.BusReviews
                    WHERE BusId = @BusId;
                    RETURN ISNULL(@Rating, 0);
                END;
GO
/****** Object:  Table [dbo].[Buses]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buses](
	[BusId] [int] IDENTITY(1,1) NOT NULL,
	[Capacity] [int] NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[Number] [nvarchar](20) NOT NULL,
	[DriverId] [int] NOT NULL,
 CONSTRAINT [PK_Buses] PRIMARY KEY CLUSTERED 
(
	[BusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ContactPhone] [nvarchar](20) NULL,
	[ContactEmail] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drivers]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drivers](
	[DriverId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DriverLicense] [nvarchar](50) NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_Drivers] PRIMARY KEY CLUSTERED 
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[BusDetailsView]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[BusDetailsView]
WITH SCHEMABINDING
                    AS
                    SELECT 
                        b.BusId,
                        b.Number,
                        b.Brand,
	                    b.Capacity,
	                    CONCAT(d.FirstName, ' ', d.LastName) AS DriverName,
	                    c.Name AS CompanyName,
                        dbo.CalculateBusRating(b.BusId) AS Rating
                    FROM dbo.Buses AS b
	                    JOIN dbo.Drivers AS d
		                    ON b.DriverId = d.DriverId
	                    JOIN dbo.Companies AS c
		                    ON d.CompanyId = c.CompanyId;
GO
/****** Object:  Table [dbo].[Trips]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trips](
	[TripId] [int] IDENTITY(1,1) NOT NULL,
	[BusScheduleEntryId] [int] NOT NULL,
	[DepartureTime] [datetime2](7) NULL,
	[ArrivalTime] [datetime2](7) NULL,
	[Status] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Trips] PRIMARY KEY CLUSTERED 
(
	[TripId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TicketPayments]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketPayments](
	[TicketPaymentId] [int] IDENTITY(1,1) NOT NULL,
	[PassengerId] [int] NOT NULL,
	[TicketId] [int] NOT NULL,
	[PaymentType] [nvarchar](20) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[TripId] [int] NOT NULL,
 CONSTRAINT [PK_TicketPayments] PRIMARY KEY CLUSTERED 
(
	[TicketPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusScheduleEntries]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusScheduleEntries](
	[BusScheduleEntryId] [int] IDENTITY(1,1) NOT NULL,
	[BusId] [int] NOT NULL,
	[RouteId] [int] NOT NULL,
	[DepartureTime] [time](7) NOT NULL,
	[ArrivalTime] [time](7) NOT NULL,
	[MoveDirection] [nvarchar](20) NOT NULL,
	[Day] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_BusScheduleEntries] PRIMARY KEY CLUSTERED 
(
	[BusScheduleEntryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateFreeSeats]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE   FUNCTION [dbo].[CalculateFreeSeats](@tripId INT)
                RETURNS INT
                WITH SCHEMABINDING
                AS
                BEGIN
                    DECLARE @totalSeats INT;
                    DECLARE @boughtTicketsCount INT;

                    -- Calculate total number of seats for the bus associated with the trip
                    SELECT @totalSeats = b.Capacity
                    FROM dbo.Trips t
                    INNER JOIN dbo.BusScheduleEntries bse ON t.BusScheduleEntryId = bse.BusScheduleEntryId
                    INNER JOIN dbo.Buses b ON bse.BusId = b.BusId
                    WHERE t.TripId = @tripId;

                    -- Calculate count of bought tickets for the trip
                    SELECT @boughtTicketsCount = COUNT(*)
                    FROM dbo.TicketPayments
                    WHERE TripId = @tripId;

                    RETURN @totalSeats - @boughtTicketsCount;
                END;
GO
/****** Object:  Table [dbo].[Routes]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Routes](
	[RouteId] [int] IDENTITY(1,1) NOT NULL,
	[Origin] [nvarchar](50) NOT NULL,
	[Destination] [nvarchar](50) NOT NULL,
	[OverallDistance]  AS ([dbo].[CalculateTotalDistanceForRoute]([RouteId])),
 CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED 
(
	[RouteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TripView]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

            create   view [dbo].[TripView]
                WITH SCHEMABINDING AS
                select
                    t.TripId,
                    t.DepartureTime,
                    t.ArrivalTime,
                    r.Origin + ' - ' + r.Destination as RouteName,
                    b.Brand as BusBrand,
                    b.CompanyName,
                    b.Rating as BusRating,
                    t.Status as TripStatus,
                    dbo.CalculateFreeSeats(t.TripId) as FreeSeatsCount,
					b.Capacity
                from dbo.Trips as t
                    join dbo.BusScheduleEntries as bse
                        on t.BusScheduleEntryId = bse.BusScheduleEntryId
                    join dbo.Routes as r
                        on bse.RouteId = r.RouteId
                    join dbo.BusDetailsView as b
                        on bse.BusId = b.BusId
GO
/****** Object:  Table [dbo].[Passengers]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Passengers](
	[PassengerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK_Passengers] PRIMARY KEY CLUSTERED 
(
	[PassengerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stops]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stops](
	[StopId] [int] IDENTITY(1,1) NOT NULL,
	[RouteId] [int] NOT NULL,
	[PreviousStopId] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DistanceToPrevious] [int] NULL,
 CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED 
(
	[StopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tickets](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[SeatId] [int] NOT NULL,
	[IsSold] [bit] NOT NULL,
	[StartStopId] [int] NOT NULL,
	[EndStopId] [int] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[Price]  AS ([dbo].[CalculateNewTicketPrice]([StartStopId],[EndStopId])),
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[PaymentsView]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE VIEW [dbo].[PaymentsView]
                AS
                SELECT tp.TicketPaymentId,
                    tp.PaymentDate,
                    tp.PaymentType,
                    (p.FirstName + ' ' + p.LastName) AS BoughtBy,
                    t.Price,
                    tr.RouteName,
                    ss.Name AS StartStop,
                    es.Name AS EndStop
                FROM TicketPayments as tp
                JOIN Tickets AS t ON tp.TicketId = t.TicketId
                JOIN Passengers AS p ON tp.PassengerId = p.PassengerId
                JOIN TripView AS tr ON tp.TripId = tr.TripId
                JOIN Stops AS ss ON t.StartStopId = ss.StopId
                JOIN Stops AS es ON t.EndStopId = es.StopId
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seats]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seats](
	[SeatId] [int] IDENTITY(1,1) NOT NULL,
	[BusId] [int] NOT NULL,
	[SeatNumber] [int] NOT NULL,
	[IsFree] [bit] NOT NULL,
 CONSTRAINT [PK_Seats] PRIMARY KEY CLUSTERED 
(
	[SeatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Role] [int] NOT NULL,
	[PasswordHash] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[LastLoginDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Buses_DriverId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Buses_DriverId] ON [dbo].[Buses]
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Buses_Number]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Buses_Number] ON [dbo].[Buses]
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusReviews_BusId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusReviews_BusId] ON [dbo].[BusReviews]
(
	[BusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusReviews_PassengerId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusReviews_PassengerId] ON [dbo].[BusReviews]
(
	[PassengerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusScheduleEntries_ArrivalTime]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusScheduleEntries_ArrivalTime] ON [dbo].[BusScheduleEntries]
(
	[ArrivalTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusScheduleEntries_BusId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusScheduleEntries_BusId] ON [dbo].[BusScheduleEntries]
(
	[BusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BusScheduleEntries_Day]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusScheduleEntries_Day] ON [dbo].[BusScheduleEntries]
(
	[Day] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusScheduleEntries_DepartureTime]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusScheduleEntries_DepartureTime] ON [dbo].[BusScheduleEntries]
(
	[DepartureTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusScheduleEntries_RouteId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_BusScheduleEntries_RouteId] ON [dbo].[BusScheduleEntries]
(
	[RouteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Drivers_CompanyId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Drivers_CompanyId] ON [dbo].[Drivers]
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Drivers_FirstName]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Drivers_FirstName] ON [dbo].[Drivers]
(
	[FirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Drivers_LastName]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Drivers_LastName] ON [dbo].[Drivers]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Passengers_FirstName]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Passengers_FirstName] ON [dbo].[Passengers]
(
	[FirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Passengers_LastName]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Passengers_LastName] ON [dbo].[Passengers]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Seats_BusId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Seats_BusId] ON [dbo].[Seats]
(
	[BusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Stops_PreviousStopId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Stops_PreviousStopId] ON [dbo].[Stops]
(
	[PreviousStopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Stops_RouteId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Stops_RouteId] ON [dbo].[Stops]
(
	[RouteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TicketPayments_PassengerId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_TicketPayments_PassengerId] ON [dbo].[TicketPayments]
(
	[PassengerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TicketPayments_TicketId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TicketPayments_TicketId] ON [dbo].[TicketPayments]
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TicketPayments_TripId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_TicketPayments_TripId] ON [dbo].[TicketPayments]
(
	[TripId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tickets_EndStopId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Tickets_EndStopId] ON [dbo].[Tickets]
(
	[EndStopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tickets_SeatId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Tickets_SeatId] ON [dbo].[Tickets]
(
	[SeatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tickets_StartStopId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Tickets_StartStopId] ON [dbo].[Tickets]
(
	[StartStopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Trips_BusScheduleEntryId]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Trips_BusScheduleEntryId] ON [dbo].[Trips]
(
	[BusScheduleEntryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Trips_Status]    Script Date: 10.05.2024 7:22:19 ******/
CREATE NONCLUSTERED INDEX [IX_Trips_Status] ON [dbo].[Trips]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Buses] ADD  DEFAULT ((0)) FOR [DriverId]
GO
ALTER TABLE [dbo].[BusReviews] ADD  DEFAULT ((0)) FOR [ComfortRating]
GO
ALTER TABLE [dbo].[BusReviews] ADD  DEFAULT ((0)) FOR [PunctualityRating]
GO
ALTER TABLE [dbo].[BusReviews] ADD  DEFAULT ((0)) FOR [PriceQualityRatioRating]
GO
ALTER TABLE [dbo].[BusReviews] ADD  DEFAULT ((0)) FOR [InternetConnectionRating]
GO
ALTER TABLE [dbo].[BusReviews] ADD  DEFAULT ((0)) FOR [SanitaryConditionsRating]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [City]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [Country]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [Street]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [ZipCode]
GO
ALTER TABLE [dbo].[Drivers] ADD  DEFAULT ((0)) FOR [CompanyId]
GO
ALTER TABLE [dbo].[Stops] ADD  DEFAULT ((0)) FOR [RouteId]
GO
ALTER TABLE [dbo].[TicketPayments] ADD  DEFAULT ((0)) FOR [PassengerId]
GO
ALTER TABLE [dbo].[TicketPayments] ADD  DEFAULT ((0)) FOR [TicketId]
GO
ALTER TABLE [dbo].[TicketPayments] ADD  DEFAULT ((0)) FOR [TripId]
GO
ALTER TABLE [dbo].[Tickets] ADD  DEFAULT (N'') FOR [Status]
GO
ALTER TABLE [dbo].[Trips] ADD  DEFAULT (N'') FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [LastLoginDate]
GO
ALTER TABLE [dbo].[Buses]  WITH CHECK ADD  CONSTRAINT [FK_Buses_Drivers_DriverId] FOREIGN KEY([DriverId])
REFERENCES [dbo].[Drivers] ([DriverId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Buses] CHECK CONSTRAINT [FK_Buses_Drivers_DriverId]
GO
ALTER TABLE [dbo].[BusReviews]  WITH CHECK ADD  CONSTRAINT [FK_BusReviews_Buses_BusId] FOREIGN KEY([BusId])
REFERENCES [dbo].[Buses] ([BusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusReviews] CHECK CONSTRAINT [FK_BusReviews_Buses_BusId]
GO
ALTER TABLE [dbo].[BusReviews]  WITH CHECK ADD  CONSTRAINT [FK_BusReviews_Passengers_PassengerId] FOREIGN KEY([PassengerId])
REFERENCES [dbo].[Passengers] ([PassengerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusReviews] CHECK CONSTRAINT [FK_BusReviews_Passengers_PassengerId]
GO
ALTER TABLE [dbo].[BusScheduleEntries]  WITH CHECK ADD  CONSTRAINT [FK_BusScheduleEntries_Buses_BusId] FOREIGN KEY([BusId])
REFERENCES [dbo].[Buses] ([BusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusScheduleEntries] CHECK CONSTRAINT [FK_BusScheduleEntries_Buses_BusId]
GO
ALTER TABLE [dbo].[BusScheduleEntries]  WITH CHECK ADD  CONSTRAINT [FK_BusScheduleEntries_Routes_RouteId] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([RouteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusScheduleEntries] CHECK CONSTRAINT [FK_BusScheduleEntries_Routes_RouteId]
GO
ALTER TABLE [dbo].[Drivers]  WITH CHECK ADD  CONSTRAINT [FK_Drivers_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Drivers] CHECK CONSTRAINT [FK_Drivers_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Seats]  WITH CHECK ADD  CONSTRAINT [FK_Seats_Buses_BusId] FOREIGN KEY([BusId])
REFERENCES [dbo].[Buses] ([BusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Seats] CHECK CONSTRAINT [FK_Seats_Buses_BusId]
GO
ALTER TABLE [dbo].[Stops]  WITH CHECK ADD  CONSTRAINT [FK_Stops_Routes_RouteId] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([RouteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stops] CHECK CONSTRAINT [FK_Stops_Routes_RouteId]
GO
ALTER TABLE [dbo].[Stops]  WITH CHECK ADD  CONSTRAINT [FK_Stops_Stops_PreviousStopId] FOREIGN KEY([PreviousStopId])
REFERENCES [dbo].[Stops] ([StopId])
GO
ALTER TABLE [dbo].[Stops] CHECK CONSTRAINT [FK_Stops_Stops_PreviousStopId]
GO
ALTER TABLE [dbo].[TicketPayments]  WITH CHECK ADD  CONSTRAINT [FK_TicketPayments_Passengers_PassengerId] FOREIGN KEY([PassengerId])
REFERENCES [dbo].[Passengers] ([PassengerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TicketPayments] CHECK CONSTRAINT [FK_TicketPayments_Passengers_PassengerId]
GO
ALTER TABLE [dbo].[TicketPayments]  WITH CHECK ADD  CONSTRAINT [FK_TicketPayments_Tickets_TicketId] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Tickets] ([TicketId])
GO
ALTER TABLE [dbo].[TicketPayments] CHECK CONSTRAINT [FK_TicketPayments_Tickets_TicketId]
GO
ALTER TABLE [dbo].[TicketPayments]  WITH CHECK ADD  CONSTRAINT [FK_TicketPayments_Trips_TripId] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trips] ([TripId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TicketPayments] CHECK CONSTRAINT [FK_TicketPayments_Trips_TripId]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Seats_SeatId] FOREIGN KEY([SeatId])
REFERENCES [dbo].[Seats] ([SeatId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Seats_SeatId]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Stops_EndStopId] FOREIGN KEY([EndStopId])
REFERENCES [dbo].[Stops] ([StopId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Stops_EndStopId]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Stops_StartStopId] FOREIGN KEY([StartStopId])
REFERENCES [dbo].[Stops] ([StopId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Stops_StartStopId]
GO
ALTER TABLE [dbo].[Trips]  WITH CHECK ADD  CONSTRAINT [FK_Trips_BusScheduleEntries_BusScheduleEntryId] FOREIGN KEY([BusScheduleEntryId])
REFERENCES [dbo].[BusScheduleEntries] ([BusScheduleEntryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trips] CHECK CONSTRAINT [FK_Trips_BusScheduleEntries_BusScheduleEntryId]
GO
/****** Object:  StoredProcedure [dbo].[AllowSwitchContextForOtherUsers]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

				CREATE PROCEDURE [dbo].[AllowSwitchContextForOtherUsers]
				(@Username NVARCHAR(50))
				AS
				BEGIN
					-- temporary table
					CREATE TABLE #TempUsers
					(PrincipalName NVARCHAR(100),RoleName NVARCHAR(100));
					
					-- cte
					WITH PrincipalsWithRoles AS (SELECT *
								FROM (SELECT P.name AS PrincipalName,R.role_principal_id AS GH
									FROM SYS.database_principals P,sys.database_role_members R
									WHERE P.principal_id=R.member_principal_id OR P.principal_id=R.role_principal_id
									AND type<>'R') S INNER JOIN (SELECT P.name AS RoleName,P.principal_id AS GHA
																	FROM SYS.database_principals P,sys.database_role_members R
																	WHERE P.principal_id=R.member_principal_id OR P.principal_id=R.role_principal_id
																	AND type='R') D
									ON D.GHA=S.GH AND PrincipalName != 'dbo' AND PrincipalName != @Username)
					
					INSERT INTO #TempUsers
					SELECT DISTINCT PrincipalName,RoleName
					FROM PrincipalsWithRoles
					
					DECLARE @CurrentUser NVARCHAR(100), @QueryText NVARCHAR(MAX)=''
					
					DECLARE UserCursor CURSOR FOR SELECT PrincipalName FROM #TempUsers
					OPEN UserCursor
					FETCH NEXT FROM UserCursor INTO @CurrentUser
					WHILE(@@FETCH_STATUS = 0)
						BEGIN
							SET @QueryText += 'GRANT IMPERSONATE ON USER::[' + @CurrentUser + '] TO ' + @Username + '; '
							FETCH NEXT FROM UserCursor INTO @CurrentUser
						END
					CLOSE UserCursor
					DEALLOCATE UserCursor
					
					DROP TABLE #TempUsers
					EXEC(@QueryText)
				END
GO
/****** Object:  StoredProcedure [dbo].[CreateUserWithRole]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

				CREATE PROCEDURE [dbo].[CreateUserWithRole]
				( @Username NVARCHAR(50),
				  @Password NVARCHAR(50),
				  @RoleName INT)
				AS
				BEGIN
					DECLARE @LoginName NVARCHAR(50) = @Username + 'Login';
					DECLARE @LoginCommand NVARCHAR(100) = 'CREATE LOGIN '+ @LoginName + ' WITH PASSWORD = '''+ @Password +'''';
					EXEC(@LoginCommand)
					
					DECLARE @UserCommand NVARCHAR(100) = 'CREATE USER ' + @Username + ' FOR LOGIN ' + @LoginName;
					EXEC(@UserCommand)

					IF @RoleName = 100 -- admin
					BEGIN
						EXEC sp_addrolemember 'db_owner', @Username
					END
					IF @RoleName = 200
					BEGIN
						EXEC sp_addrolemember 'Passenger', @Username
					END
					IF @RoleName = 300
					BEGIN
						EXEC sp_addrolemember 'Driver', @Username
					END

					EXEC AllowSwitchContextForOtherUsers @Username;
				END
GO
/****** Object:  StoredProcedure [dbo].[GenerateSeatsForBus]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE PROCEDURE [dbo].[GenerateSeatsForBus]
                    @BusId INT,
                    @SeatCount INT
                AS
                BEGIN
                    DECLARE @SeatNumber INT = 1;

                    WHILE @SeatNumber <= @SeatCount
                    BEGIN
                        INSERT INTO Seats (BusId, SeatNumber, IsFree)
                        VALUES (@BusId, @SeatNumber, 1); -- Assuming all seats are initially free

                        SET @SeatNumber = @SeatNumber + 1;
                    END;
                END;
GO
/****** Object:  StoredProcedure [dbo].[GetFirstAndLastRouteStops]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE   PROCEDURE [dbo].[GetFirstAndLastRouteStops]
                    @RouteId INT,
                    @FirstStopId INT OUTPUT,
                    @LastStopId INT OUTPUT
                AS
                BEGIN
                    DECLARE @CurrentStopId INT;
                    DECLARE @NextStopId INT;

                    -- Set first stop on route
                    SELECT @CurrentStopId = StopId
                    FROM Stops
                    WHERE PreviousStopId IS NULL AND RouteId = @RouteId;
                    SET @FirstStopId = @CurrentStopId;

                    -- Traverse stops to find the last stop
                    WHILE @CurrentStopId IS NOT NULL
                    BEGIN
                        -- Update the last stop ID
                        SET @LastStopId = @CurrentStopId;
                        -- Get the next stop ID
                        SET @NextStopId = dbo.FindNextStop(@RouteId, @CurrentStopId);
                        -- Update the current stop ID
                        SET @CurrentStopId = @NextStopId;
                    END;
                END;
GO
/****** Object:  StoredProcedure [dbo].[HandleSeatAvailability]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                CREATE PROC [dbo].[HandleSeatAvailability]
                (   @TicketId INT,
                    @NewStatus NVARCHAR(20))
                AS
                BEGIN
	                -- set seat state based on new ticket status
	                DECLARE @IsFree BIT;

                     -- check new status
                    IF @NewStatus = 'Active' OR @NewStatus = 'Cancelled'
                        SET @IsFree = 1;
                    ELSE
                        SET @IsFree = 0;

                    -- update corresponding seat
                    UPDATE Seats
                    SET IsFree = @IsFree
                    WHERE SeatId = (SELECT SeatId FROM Tickets WHERE TicketId = @TicketId);
                END;
GO
/****** Object:  StoredProcedure [dbo].[SeedPermissionsForRoles]    Script Date: 10.05.2024 7:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

				CREATE PROCEDURE [dbo].[SeedPermissionsForRoles]
				AS
				BEGIN
					CREATE ROLE Passenger;
					GRANT SELECT ON Buses TO Passenger;
					GRANT SELECT ON Routes TO Passenger;
					GRANT SELECT ON Stops TO Passenger;
					GRANT SELECT ON Trips TO Passenger;
					GRANT SELECT ON TripStatuses TO Passenger;
					GRANT SELECT ON BusDetailsView TO Passenger;
					GRANT SELECT, INSERT, UPDATE, DELETE ON BusReviews TO Passenger;
					GRANT SELECT, UPDATE ON Seats TO Passenger;
					GRANT SELECT, UPDATE, INSERT, DELETE ON Tickets TO Passenger;
					GRANT SELECT, UPDATE, INSERT, DELETE ON TicketStatuses TO Passenger;
					GRANT SELECT, UPDATE, INSERT ON TicketPayments TO Passenger;
					GRANT SELECT, UPDATE, INSERT ON Users TO Passenger;

					CREATE ROLE Driver;
					GRANT SELECT, UPDATE ON Buses TO Driver;
					GRANT SELECT, UPDATE ON Stops TO Driver;
					GRANT SELECT ON Tickets TO Driver;
					GRANT SELECT ON Companies TO Driver;
					GRANT SELECT, UPDATE, INSERT, DELETE ON BusScheduleEntries TO Driver;
					GRANT SELECT ON BusDetailsView TO Driver;
					GRANT SELECT ON BusReviews TO Driver;
					GRANT SELECT ON Passengers TO Driver;
					GRANT SELECT, DELETE ON Seats TO Driver;
					GRANT SELECT, UPDATE, INSERT, DELETE ON Trips TO Driver;
					GRANT SELECT, UPDATE, INSERT ON TripStatuses TO Driver;
					GRANT SELECT ON TicketPayments TO Driver;
					GRANT SELECT, UPDATE, INSERT ON Users TO Driver;
				END
GO
USE [master]
GO
ALTER DATABASE [BusStationDB] SET  READ_WRITE 
GO
