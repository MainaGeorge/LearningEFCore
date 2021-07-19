using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore5WebApp.DAL.Migrations
{
    public partial class AddingStoredProcedureToTheDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PersonPerson",
                newName: "PersonPerson",
                newSchema: "dbo");

            var procedure1 = @"
                IF OBJECT_ID('GetPersonsByState', 'P') IS NOT NULL
                DROP PROC GetPersonsByState
                GO
                CREATE PROCEDURE [dbo].[GetPersonsByState]
                @State varchar(255)
                AS
                BEGIN
                SELECT p.*
                FROM Persons p
                INNER JOIN Addresses a on p.Id = a.PersonId
                WHERE a.State = @State
                END";

            var procedure2 = @"
                IF OBJECT_ID('AddLookUpItem', 'P') IS NOT NULL
                DROP PROC AddLookUpItem
                GO
                CREATE PROCEDURE [dbo].[AddLookUpItem]
                @Code varchar(255),
                @Description varchar(255),
                @LookUpTypeId int
                AS
                BEGIN
                INSERT INTO LookUps (Code, Description, LookUpType) VALUES (@Code,
                @Description, @LookUpTypeId)
                END";

            migrationBuilder.Sql(procedure2);
            migrationBuilder.Sql(procedure1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PersonPerson",
                schema: "dbo",
                newName: "PersonPerson");

            migrationBuilder.Sql(@"DROP PROC GetPersonsByState");
            migrationBuilder.Sql(@"DROP PROC AddLookUpItem");
        }
    }
}
