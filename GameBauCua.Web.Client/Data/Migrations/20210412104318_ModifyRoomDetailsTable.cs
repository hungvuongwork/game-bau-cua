using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class ModifyRoomDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "RoomDetails");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "RoomDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "RoomDetails");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "RoomDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
