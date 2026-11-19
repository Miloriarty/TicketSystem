using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticeProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultAdmin : Migration
    {
        private const string PasswordHash =
            "AQAAAAIAAYagAAAAEIbzsGjzx87APLoxnnKFo87EmJ6yyGpxVmHnO1zOE9pf0A8IbAy6PjxLZSvysT0Z2w==";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                INSERT INTO "Employees" ("FullName", "Position", "Password", "Role", "StartDate")
                SELECT 'admin', 'Administrator', '{PasswordHash}', 'admin', now()
                WHERE NOT EXISTS (SELECT 1 FROM "Employees" WHERE "FullName" = 'admin');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DELETE FROM "Employees"
                WHERE "FullName" = 'admin' AND "Password" = '{PasswordHash}';
                """);
        }
    }
}
