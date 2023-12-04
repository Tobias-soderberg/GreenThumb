using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumb.Migrations
{
    public partial class InitialUpdatedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GardenPlant_GardenModel_garden_id",
                table: "GardenPlant");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_GardenModel_garden_id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GardenModel",
                table: "GardenModel");

            migrationBuilder.RenameTable(
                name: "GardenModel",
                newName: "Gardens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gardens",
                table: "Gardens",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GardenPlant_Gardens_garden_id",
                table: "GardenPlant",
                column: "garden_id",
                principalTable: "Gardens",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Gardens_garden_id",
                table: "Users",
                column: "garden_id",
                principalTable: "Gardens",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GardenPlant_Gardens_garden_id",
                table: "GardenPlant");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Gardens_garden_id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gardens",
                table: "Gardens");

            migrationBuilder.RenameTable(
                name: "Gardens",
                newName: "GardenModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GardenModel",
                table: "GardenModel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GardenPlant_GardenModel_garden_id",
                table: "GardenPlant",
                column: "garden_id",
                principalTable: "GardenModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GardenModel_garden_id",
                table: "Users",
                column: "garden_id",
                principalTable: "GardenModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
