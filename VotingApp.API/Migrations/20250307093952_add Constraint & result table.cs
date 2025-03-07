using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class addConstraintresulttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Candidates_StateId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "Voters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Voters");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Candidates");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Voters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTimeNow = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_StateId_PartyId",
                table: "Candidates",
                columns: new[] { "StateId", "PartyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoterId",
                table: "Votes",
                column: "VoterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_StateId_PartyId",
                table: "Candidates");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Voters",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "Voters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Voters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VoteCount",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_StateId",
                table: "Candidates",
                column: "StateId");
        }
    }
}
