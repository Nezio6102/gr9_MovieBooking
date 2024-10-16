using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR9_MovieBooking.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "378efa4e-b00c-437e-b569-50cbe3d29836", "6f75bb9b-9f22-47a9-8d7b-fdf04598250f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "378efa4e-b00c-437e-b569-50cbe3d29836");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f75bb9b-9f22-47a9-8d7b-fdf04598250f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e52d9456-e380-4c41-89bb-a2b8f5d66b97", "fbec1e3f-76e2-4a35-b0a1-ac6aaeac99ee", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "663303fb-3371-4521-840c-7dde219cffbb", 0, "b6e7b26c-768b-4002-bee4-c65d1612a11f", "blackcatinnight0102@gmail.com", true, true, null, "BLACKCATINNIGHT0102@GMAIL.COM", "NEKOMANCER", "AQAAAAEAACcQAAAAEMQ7K6zXHa8e9oHiYeZGOl2jWL650bUebk8GZhiRhzSASfD+qiCUjor4qJE7OvBFdg==", null, false, "7c329331-46e3-4aa7-b604-28257f0d3ab7", false, "nekomancer" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e52d9456-e380-4c41-89bb-a2b8f5d66b97", "663303fb-3371-4521-840c-7dde219cffbb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e52d9456-e380-4c41-89bb-a2b8f5d66b97", "663303fb-3371-4521-840c-7dde219cffbb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e52d9456-e380-4c41-89bb-a2b8f5d66b97");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "663303fb-3371-4521-840c-7dde219cffbb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "378efa4e-b00c-437e-b569-50cbe3d29836", "c72c76cc-1625-4df9-9a8e-88c7d0bd11aa", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6f75bb9b-9f22-47a9-8d7b-fdf04598250f", 0, "194b097b-1ae0-4473-a270-b4e729f18b2d", "blackcatinnight0102@gmail.com", true, true, null, "BLACKCATINNIGHT0102@GMAIL.COM", "NEKOMANCER", "AQAAAAEAACcQAAAAEIuDibXq1q4O/SG66fVp56w487zUFFUeUSqtPAyNXeJs1wc04czZrDzTHx9tIdfnXA==", null, false, "41fba910-fcf1-40d7-85d2-a3238fa81a01", false, "nekomancer" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "378efa4e-b00c-437e-b569-50cbe3d29836", "6f75bb9b-9f22-47a9-8d7b-fdf04598250f" });
        }
    }
}
