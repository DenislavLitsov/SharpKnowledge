using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SharpKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBiasesToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biases",
                table: "BrainModels");

            migrationBuilder.CreateTable(
                name: "Bias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Biases = table.Column<float[]>(type: "real[]", nullable: false),
                    BrainModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bias_BrainModels_BrainModelId",
                        column: x => x.BrainModelId,
                        principalTable: "BrainModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bias_BrainModelId",
                table: "Bias",
                column: "BrainModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bias");

            migrationBuilder.AddColumn<float[,]>(
                name: "Biases",
                table: "BrainModels",
                type: "real[]",
                nullable: false,
                defaultValue: new float[0]);
        }
    }
}
