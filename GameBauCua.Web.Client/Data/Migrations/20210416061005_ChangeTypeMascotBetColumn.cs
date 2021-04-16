using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class ChangeTypeMascotBetColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MascotBet",
                table: "RoundPlayDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MascotBet",
                table: "RoundPlayDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
