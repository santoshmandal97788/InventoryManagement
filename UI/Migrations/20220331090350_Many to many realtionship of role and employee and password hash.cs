using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UI.Migrations
{
    public partial class Manytomanyrealtionshipofroleandemployeeandpasswordhash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_listItems_ListItemCategories_ListItemCategoryId",
                table: "listItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_listItems_GenderListItemId",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_listItems",
                table: "listItems");

            migrationBuilder.RenameTable(
                name: "listItems",
                newName: "ListItems");

            migrationBuilder.RenameIndex(
                name: "IX_listItems_ListItemName",
                table: "ListItems",
                newName: "IX_ListItems_ListItemName");

            migrationBuilder.RenameIndex(
                name: "IX_listItems_ListItemCategoryId",
                table: "ListItems",
                newName: "IX_ListItems_ListItemCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems",
                column: "ListItemId");

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    EmployeeRoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.EmployeeRoleId);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EmployeeRoles",
                columns: new[] { "EmployeeRoleId", "EmployeeId", "RoleId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "Password",
                value: "GQDrrNPfW+tsNZ/N+PQQpRaZms5ia1DRPN3dIfgDt04=");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_EmployeeId",
                table: "EmployeeRoles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_RoleId",
                table: "EmployeeRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_ListItemCategories_ListItemCategoryId",
                table: "ListItems",
                column: "ListItemCategoryId",
                principalTable: "ListItemCategories",
                principalColumn: "ListItemCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_ListItems_GenderListItemId",
                table: "Persons",
                column: "GenderListItemId",
                principalTable: "ListItems",
                principalColumn: "ListItemId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_ListItemCategories_ListItemCategoryId",
                table: "ListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_ListItems_GenderListItemId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems");

            migrationBuilder.RenameTable(
                name: "ListItems",
                newName: "listItems");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_ListItemName",
                table: "listItems",
                newName: "IX_listItems_ListItemName");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_ListItemCategoryId",
                table: "listItems",
                newName: "IX_listItems_ListItemCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_listItems",
                table: "listItems",
                column: "ListItemId");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "Password",
                value: "admin");

            migrationBuilder.AddForeignKey(
                name: "FK_listItems_ListItemCategories_ListItemCategoryId",
                table: "listItems",
                column: "ListItemCategoryId",
                principalTable: "ListItemCategories",
                principalColumn: "ListItemCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_listItems_GenderListItemId",
                table: "Persons",
                column: "GenderListItemId",
                principalTable: "listItems",
                principalColumn: "ListItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
