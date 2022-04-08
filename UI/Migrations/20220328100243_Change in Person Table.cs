using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UI.Migrations
{
    public partial class ChangeinPersonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Persons_GenderListItemId",
                table: "Persons",
                column: "GenderListItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_listItems_GenderListItemId",
                table: "Persons",
                column: "GenderListItemId",
                principalTable: "listItems",
                principalColumn: "ListItemId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_listItems_GenderListItemId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_GenderListItemId",
                table: "Persons");
        }
    }
}
