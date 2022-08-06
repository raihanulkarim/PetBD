using Microsoft.EntityFrameworkCore.Migrations;

namespace PetBD.Migrations
{
    public partial class initproductionmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4ad64310-f2bd-46ce-bb04-cd0a6c45b6a6", "AQAAAAEAACcQAAAAEMCBaJcEjHufO/9mAVUL28NsuHT3/DEUjX1zjI5XoN3XT1Je10DuLW7ioN0PdEwsGw==", "c0cac617-ec3f-4f79-b86b-a0dc96512439" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a524b444-f349-44d4-9c88-0c64e2828595", "AQAAAAEAACcQAAAAEE7DifQwWJjK1aS1bSUwDpOyB4y+O9cQ39d2ujhepSRp19oumqqJm9xsBmHEWJ4uvg==", "4f864f54-89a6-42a9-95da-0c7e3e7ce14b" });
        }
    }
}
