using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousStopDistanceFieldToStopsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistanceToPrevious",
                table: "Stops",
                type: "int",
                nullable: true);
            
            migrationBuilder.Sql(@"
                CREATE FUNCTION CalculateTotalDistanceForRoute
                (@RouteId INT)
                RETURNS INT
                AS
                BEGIN
                    DECLARE @OverallDistance INT = 0;

                    SELECT @OverallDistance = SUM(ISNULL(DistanceToPrevious, 0))
                    FROM Stops
                    WHERE RouteId = @RouteId;

                RETURN @OverallDistance;
                END;
                GO");

                migrationBuilder.Sql(@"
                    ALTER   TRIGGER [dbo].[InsertOriginAndDestinationStopsForRoute]
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
                        VALUES(@InsertedRouteId, NULL, @OriginStopName, NULL);
                        
                        -- insert last stop
                        DECLARE @PrevStopId INT = SCOPE_IDENTITY();

                        DECLARE @DefaultDistance INT = 20; --experiment
                        INSERT INTO Stops
                        VALUES(@InsertedRouteId, @PrevStopId, @DestinationStopName, @DefaultDistance);

                        ENABLE TRIGGER HandleStopOrderOnInsert ON Stops;
                    END;
                    GO");

                migrationBuilder.Sql(@"
                ALTER   TRIGGER [dbo].[HandleStopOrderOnInsert]
                ON [dbo].[Stops]
                INSTEAD OF INSERT
                AS
                BEGIN
                    -- always append new stop in the end (just before the destination point)
                    DECLARE @RouteId INT;
                    DECLARE @InsertedStopId INT;
                    DECLARE @FirstStopId INT;
                    DECLARE @LastStopId INT;
					DECLARE @Distance INT;
                    
                    SELECT @RouteId = RouteId FROM inserted;
                    
                    EXEC dbo.GetFirstAndLastRouteStops @RouteId, @FirstStopId OUTPUT, @LastStopId OUTPUT;
                    
                    -- Insert the new stop
                    INSERT INTO Stops (RouteId, PreviousStopId, Name, DistanceToPrevious)
                    SELECT RouteId, NULL, Name, DistanceToPrevious FROM inserted;
                    SET @InsertedStopId = SCOPE_IDENTITY()
                    
                    -- get previous stop of last stop
                    DECLARE @PrevOfLastStopId INT = 
                    (SELECT PreviousStopId FROM Stops WHERE StopId = @LastStopId);
                    
                    -- update new stop
                    UPDATE Stops
                    SET PreviousStopId = @PrevOfLastStopId
                    WHERE StopId = @InsertedStopId

                    -- update last stop
					SELECT @Distance = DistanceToPrevious FROM inserted;

					UPDATE Stops
                    SET PreviousStopId = @InsertedStopId,
						DistanceToPrevious = ISNULL(DistanceToPrevious, 0) + @Distance
                    WHERE StopId = @LastStopId
                    
                END;
                GO");
                
                migrationBuilder.Sql(@"
                ALTER   TRIGGER [dbo].[HandleStopDeletion]
                ON [dbo].[Stops]
                AFTER DELETE
                AS
                BEGIN
                    DECLARE @RouteId INT;
                    DECLARE @DeletedStopId INT;
                    DECLARE @PrevStopId INT;
                    DECLARE @NextStopId INT;
					DECLARE @Distance INT;

                    SELECT @RouteId = RouteId,
                        @DeletedStopId = StopId,
						@Distance = DistanceToPrevious
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
                        SET PreviousStopId = @PrevStopId,
							DistanceToPrevious = ISNULL(DistanceToPrevious, 0) - @Distance
                        WHERE StopId = @NextStopId;
                    END
                    ELSE
                    BEGIN
                        -- in this case delete whole route because it will be broken
                        DELETE FROM Routes WHERE RouteId = @RouteId;
                    END
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS CalculateTotalDistanceForRoute");
            
            migrationBuilder.Sql(@"
            ALTER   TRIGGER [dbo].[HandleStopDeletion]
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
                END;
                GO");

                migrationBuilder.Sql(@"
                ALTER   TRIGGER [dbo].[HandleStopOrderOnInsert]
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
                ALTER   TRIGGER [dbo].[InsertOriginAndDestinationStopsForRoute]
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

            migrationBuilder.DropColumn(
                name: "DistanceToPrevious",
                table: "Stops");
        }
    }
}
