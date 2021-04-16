using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YourCapital",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RoomStatus",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    NumberOfPlayers = table.Column<int>(nullable: false),
                    MinimumBet = table.Column<long>(nullable: false),
                    ExpectedMaximumBet = table.Column<long>(nullable: false),
                    HostId = table.Column<string>(nullable: true),
                    RoomStatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_HostId",
                        column: x => x.HostId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomStatus_RoomStatusId",
                        column: x => x.RoomStatusId,
                        principalTable: "RoomStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoundPlays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoundNumber = table.Column<int>(nullable: false),
                    MinimumBet = table.Column<int>(nullable: false),
                    MaximumBet = table.Column<int>(nullable: false),
                    WinMascots = table.Column<string>(nullable: true),
                    RoomId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundPlays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundPlays_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoundPlayDetail",
                columns: table => new
                {
                    RoundPlayId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<string>(nullable: false),
                    MascotBet = table.Column<string>(nullable: true),
                    MinimumBet = table.Column<int>(nullable: false),
                    MaximumBet = table.Column<int>(nullable: false),
                    IsReady = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundPlayDetail", x => new { x.PlayerId, x.RoundPlayId });
                    table.ForeignKey(
                        name: "FK_RoundPlayDetail_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoundPlayDetail_RoundPlays_RoundPlayId",
                        column: x => x.RoundPlayId,
                        principalTable: "RoundPlays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HostId",
                table: "Rooms",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomStatusId",
                table: "Rooms",
                column: "RoomStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundPlayDetail_RoundPlayId",
                table: "RoundPlayDetail",
                column: "RoundPlayId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundPlays_RoomId",
                table: "RoundPlays",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoundPlayDetail");

            migrationBuilder.DropTable(
                name: "RoundPlays");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomStatus");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "YourCapital",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
