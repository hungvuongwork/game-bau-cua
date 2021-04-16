using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class ModifyRoundPlayDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "RoundPlayDetails");

            migrationBuilder.DropColumn(
                name: "MaximumBet",
                table: "RoundPlayDetails");

            migrationBuilder.DropColumn(
                name: "MinimumBet",
                table: "RoundPlayDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsWaitingPlayerReady",
                table: "RoundPlays",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "YourBet",
                table: "RoundPlayDetails",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWaitingPlayerReady",
                table: "RoundPlays");

            migrationBuilder.DropColumn(
                name: "YourBet",
                table: "RoundPlayDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "RoundPlayDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaximumBet",
                table: "RoundPlayDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumBet",
                table: "RoundPlayDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
