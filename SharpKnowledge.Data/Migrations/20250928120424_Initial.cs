using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SharpKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrainModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrainType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalRuns = table.Column<long>(type: "bigint", nullable: false),
                    Generation = table.Column<int>(type: "integer", nullable: false),
                    BestScore = table.Column<float>(type: "real", nullable: false),
                    Biases = table.Column<float[,]>(type: "real[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrainModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeightData = table.Column<float[,]>(type: "real[]", nullable: false),
                    BrainModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weight_BrainModels_BrainModelId",
                        column: x => x.BrainModelId,
                        principalTable: "BrainModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weight_BrainModelId",
                table: "Weight",
                column: "BrainModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weight");

            migrationBuilder.DropTable(
                name: "BrainModels");
        }
    }
}
