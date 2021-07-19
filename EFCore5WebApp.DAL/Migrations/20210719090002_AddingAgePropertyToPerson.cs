using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore5WebApp.DAL.Migrations
{
    public partial class AddingAgePropertyToPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PersonPerson",
                newName: "PersonPerson",
                newSchema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "dbo",
                table: "persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "persons",
                keyColumn: "Id",
                keyValue: 1,
                column: "Age",
                value: 30);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "persons",
                keyColumn: "Id",
                keyValue: 2,
                column: "Age",
                value: 26);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "dbo",
                table: "persons");

            migrationBuilder.RenameTable(
                name: "PersonPerson",
                schema: "dbo",
                newName: "PersonPerson");
        }
    }
}
