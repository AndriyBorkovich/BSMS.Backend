using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProceduresForRoleBasedUserLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				CREATE PROCEDURE SeedPermissionsForRoles
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
				GO");
	        
            migrationBuilder.Sql(@"
				CREATE PROCEDURE AllowSwitchContextForOtherUsers
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
				GO");

            migrationBuilder.Sql(@"
				CREATE PROCEDURE CreateUserWithRole
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
				GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS SeedPermissionsForRoles;
				DROP PROCEDURE IF EXISTS AllowSwitchContextForOtherUsers;
				DROP PROCEDURE IF EXISTS CreateUserWithRole;");
        }
    }
}
