using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR9_MovieBooking.Migrations
{
    public partial class UpdateAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c2fe0a9c-4c87-4f65-896e-02f8ba5fa4ba", "270316c7-cbeb-4af4-a7a7-97038e67ddee" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2fe0a9c-4c87-4f65-896e-02f8ba5fa4ba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "270316c7-cbeb-4af4-a7a7-97038e67ddee");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "c2fe0a9c-4c87-4f65-896e-02f8ba5fa4ba", "25095c3a-a6ac-40e9-8f48-52b8e0384a42", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "270316c7-cbeb-4af4-a7a7-97038e67ddee", 0, "5e75e084-5649-42da-8f2a-abc04398d33d", "aimanecouissi@gmail.com", true, true, null, "AIMANECOUISSI@GMAIL.COM", "AIMANECOUISSI", "AQAAAAEAACcQAAAAEFQ5fjL18gaSbm2bS4d/vH0MKvu2AcrmFxaTSeMdAk4PeS9D0KXtD3K/eFAUPIfMHg==", null, false, "ca3192ce-d275-4e06-8497-13595817c12c", false, "aimanecouissi" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c2fe0a9c-4c87-4f65-896e-02f8ba5fa4ba", "270316c7-cbeb-4af4-a7a7-97038e67ddee" });
        }
    }
}
