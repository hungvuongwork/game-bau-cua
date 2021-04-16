using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBauCua.Web.Client.Data.Migrations
{
    public partial class ChangeNameRoundPlayDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundPlayDetail_AspNetUsers_PlayerId",
                table: "RoundPlayDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundPlayDetail_RoundPlays_RoundPlayId",
                table: "RoundPlayDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundPlayDetail",
                table: "RoundPlayDetail");

            migrationBuilder.RenameTable(
                name: "RoundPlayDetail",
                newName: "RoundPlayDetails");

            migrationBuilder.RenameIndex(
                name: "IX_RoundPlayDetail_RoundPlayId",
                table: "RoundPlayDetails",
                newName: "IX_RoundPlayDetails_RoundPlayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundPlayDetails",
                table: "RoundPlayDetails",
                columns: new[] { "PlayerId", "RoundPlayId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoundPlayDetails_AspNetUsers_PlayerId",
                table: "RoundPlayDetails",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoundPlayDetails_RoundPlays_RoundPlayId",
                table: "RoundPlayDetails",
                column: "RoundPlayId",
                principalTable: "RoundPlays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundPlayDetails_AspNetUsers_PlayerId",
                table: "RoundPlayDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundPlayDetails_RoundPlays_RoundPlayId",
                table: "RoundPlayDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundPlayDetails",
                table: "RoundPlayDetails");

            migrationBuilder.RenameTable(
                name: "RoundPlayDetails",
                newName: "RoundPlayDetail");

            migrationBuilder.RenameIndex(
                name: "IX_RoundPlayDetails_RoundPlayId",
                table: "RoundPlayDetail",
                newName: "IX_RoundPlayDetail_RoundPlayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundPlayDetail",
                table: "RoundPlayDetail",
                columns: new[] { "PlayerId", "RoundPlayId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoundPlayDetail_AspNetUsers_PlayerId",
                table: "RoundPlayDetail",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoundPlayDetail_RoundPlays_RoundPlayId",
                table: "RoundPlayDetail",
                column: "RoundPlayId",
                principalTable: "RoundPlays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
