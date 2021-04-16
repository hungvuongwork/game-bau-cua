using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class ModifyRoomAndUserDeleteRoomStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_HostId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomStatus_RoomStatusId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomStatus");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HostId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomStatusId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomStatusId",
                table: "Rooms");

            migrationBuilder.AddColumn<bool>(
                name: "IsFull",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "RoomDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "RoomDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFull",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "RoomDetails");

            migrationBuilder.AddColumn<string>(
                name: "HostId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomStatusId",
                table: "Rooms",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HostId",
                table: "Rooms",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomStatusId",
                table: "Rooms",
                column: "RoomStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_HostId",
                table: "Rooms",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomStatus_RoomStatusId",
                table: "Rooms",
                column: "RoomStatusId",
                principalTable: "RoomStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
