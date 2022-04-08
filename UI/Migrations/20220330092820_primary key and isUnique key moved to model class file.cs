using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UI.Migrations
{
    public partial class primarykeyandisUniquekeymovedtomodelclassfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonId_Email",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_ListItemCategories_ListItemCategoryName",
                table: "ListItemCategories",
                column: "ListItemCategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email_PersonId",
                table: "Employees",
                columns: new[] { "Email", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListItemCategories_ListItemCategoryName",
                table: "ListItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Email_PersonId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonId",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId_Email",
                table: "Employees",
                columns: new[] { "PersonId", "Email" },
                unique: true);
        }
    }
}
