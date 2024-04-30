using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStopAndRouteFunctionalityElements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[FindNextStop]
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
                GO");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[GetFirstAndLastRouteStops]
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
                GO");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[HandleStopOrderOnInsert]
                ON [dbo].[Stops]
                INSTEAD OF INSERT
                AS
                BEGIN
                    -- always append new stop in the end (just before the destination point)
                    DECLARE @RouteId INT;
                    DECLARE @InsertedStopId INT;
                    DECLARE @FirstStopId INT;
                    DECLARE @LastStopId INT;
                    
                    SELECT @RouteId = RouteId FROM inserted;
                    
                    EXEC dbo.GetFirstAndLastRouteStops @RouteId, @FirstStopId OUTPUT, @LastStopId OUTPUT;
                    
                    -- Insert the new stop
                    INSERT INTO Stops (RouteId, PreviousStopId, Name)
                    SELECT RouteId, NULL, Name FROM inserted;
                    SET @InsertedStopId = SCOPE_IDENTITY()
                    
                    -- get previous stop of last stop
                    DECLARE @PrevOfLastStopId INT = 
                    (SELECT PreviousStopId FROM Stops WHERE StopId = @LastStopId);
                    
                    -- update new stop
                    UPDATE Stops
                    SET PreviousStopId = @PrevOfLastStopId
                    WHERE StopId = @InsertedStopId

                    -- update last stop
                    UPDATE Stops
                    SET PreviousStopId = @InsertedStopId
                    WHERE StopId = @LastStopId
                    
                END;
                GO");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[HandleStopDeletion]
                ON [dbo].[Stops]
                AFTER DELETE
                AS
                BEGIN
                    DECLARE @RouteId INT;
                    DECLARE @DeletedStopId INT;
                    DECLARE @PrevStopId INT;
                    DECLARE @NextStopId INT;

                    SELECT @RouteId = RouteId,
                        @DeletedStopId = StopId
                    FROM deleted;

                    -- Check if the deleted stop is the first or last stop
                    IF NOT EXISTS (
                        SELECT 1
                        FROM Stops
                        WHERE RouteId = @RouteId
                        AND (StopId = @DeletedStopId OR PreviousStopId = @DeletedStopId)
                        AND (
                            PreviousStopId IS NULL -- First stop
                            OR NOT EXISTS (SELECT 1 FROM Stops WHERE PreviousStopId = @DeletedStopId) -- Last stop
                        )
                    )
                    BEGIN
                        -- Get the previous and next stops of the deleted stop
                        SELECT @PrevStopId = PreviousStopId
                        FROM Stops
                        WHERE StopId = @DeletedStopId;

                        SELECT @NextStopId = StopId
                        FROM Stops
                        WHERE PreviousStopId = @DeletedStopId;

                        -- Update the next stop's PreviousStopId to the previous stop of the deleted stop
                        UPDATE Stops
                        SET PreviousStopId = @PrevStopId
                        WHERE StopId = @NextStopId;
                    END
                    ELSE
                    BEGIN
                        -- in this case delete whole route because it will be broken
                        DELETE FROM Routes WHERE RouteId = @RouteId;
                    END
                END;");

                migrationBuilder.Sql(@"
                    CREATE OR ALTER TRIGGER [dbo].[InsertOriginAndDestinationStopsForRoute]
                    ON [dbo].[Routes]
                    AFTER INSERT
                    AS
                    BEGIN
                        DECLARE @InsertedRouteId INT;
                        DECLARE @OriginStopName NVARCHAR(50);
                        DECLARE @DestinationStopName NVARCHAR(50); 

                        SELECT @InsertedRouteId = RouteId
                        FROM inserted;

                        SELECT @OriginStopName = Origin, @DestinationStopName = Destination
                        FROM inserted;

                        -- important to disable trigger for correct insert of first two stops
                        DISABLE TRIGGER HandleStopOrderOnInsert ON Stops;
                        
                        -- insert first stop
                        INSERT INTO Stops
                        VALUES(@InsertedRouteId, NULL, @OriginStopName);
                        
                        -- insert last stop
                        DECLARE @PrevStopId INT = SCOPE_IDENTITY();
                        
                        INSERT INTO Stops
                        VALUES(@InsertedRouteId, @PrevStopId, @DestinationStopName);

                        ENABLE TRIGGER HandleStopOrderOnInsert ON Stops;
                    END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS InsertOriginAndDestinationStopsForRoute;
                DROP TRIGGER IF EXISTS HandleStopOrderOnInsert;
                DROP TRIGGER IF EXISTS HandleStopDeletion;
                DROP PROCEDURE IF EXISTS GetFirstAndLastRouteStops;
                DROP FUNCTION IF EXISTS FindNextStop;
            ");
        }
    }
}
