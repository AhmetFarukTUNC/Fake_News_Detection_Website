using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FakeNewsDetection.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPrediction2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Error",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_UserId",
                table: "Predictions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Users_UserId",
                table: "Predictions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Users_UserId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_UserId",
                table: "Predictions");

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
