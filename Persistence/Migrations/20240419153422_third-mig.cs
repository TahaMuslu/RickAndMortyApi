using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class thirdmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Locations_LocationId1",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_LocationId1",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Characters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Episodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LocationId1",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LocationId1",
                table: "Characters",
                column: "LocationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Locations_LocationId1",
                table: "Characters",
                column: "LocationId1",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
