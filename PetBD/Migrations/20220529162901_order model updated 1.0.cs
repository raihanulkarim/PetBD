using Microsoft.EntityFrameworkCore.Migrations;

namespace PetBD.Migrations
{
    public partial class ordermodelupdated10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Order",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a524b444-f349-44d4-9c88-0c64e2828595", "AQAAAAEAACcQAAAAEE7DifQwWJjK1aS1bSUwDpOyB4y+O9cQ39d2ujhepSRp19oumqqJm9xsBmHEWJ4uvg==", "4f864f54-89a6-42a9-95da-0c7e3e7ce14b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Order",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d3383b2-8e2e-43c8-9c18-85de2bbce131", "AQAAAAEAACcQAAAAEHaG2xaYYwyeNh3NQ7SkTKsZP/kElJNCU2MpfikzaMtXsCuVZKqfL84s+KZ+2j7hEg==", "868e1953-ae37-404f-83e4-2eba0541550b" });
        }
    }
}
