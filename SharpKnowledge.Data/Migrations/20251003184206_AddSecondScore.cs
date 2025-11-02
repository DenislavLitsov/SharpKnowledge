using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSecondScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SecondScoreValue",
                table: "BrainModels",
                type: "real",
                nullable: false,
                defaultValue: float.MaxValue);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondScoreValue",
                table: "BrainModels");
        }
    }
}
