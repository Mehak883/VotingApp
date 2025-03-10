using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class uniquePartyfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Parties",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Parties",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Name",
                table: "Parties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Name_Symbol",
                table: "Parties",
                columns: new[] { "Name", "Symbol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Symbol",
                table: "Parties",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parties_Name",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_Name_Symbol",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_Symbol",
                table: "Parties");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Parties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Parties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
