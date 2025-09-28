using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SharpKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixWeights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bias_BrainModels_BrainModelId",
                table: "Bias");

            migrationBuilder.DropForeignKey(
                name: "FK_Weight_BrainModels_BrainModelId",
                table: "Weight");

            migrationBuilder.DropColumn(
                name: "WeightData",
                table: "Weight");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrainModelId",
                table: "Weight",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrainModelId",
                table: "Bias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "WeightCol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeightData = table.Column<float[]>(type: "real[]", nullable: false),
                    WeightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightCol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightCol_Weight_WeightId",
                        column: x => x.WeightId,
                        principalTable: "Weight",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightCol_WeightId",
                table: "WeightCol",
                column: "WeightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bias_BrainModels_BrainModelId",
                table: "Bias",
                column: "BrainModelId",
                principalTable: "BrainModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weight_BrainModels_BrainModelId",
                table: "Weight",
                column: "BrainModelId",
                principalTable: "BrainModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bias_BrainModels_BrainModelId",
                table: "Bias");

            migrationBuilder.DropForeignKey(
                name: "FK_Weight_BrainModels_BrainModelId",
                table: "Weight");

            migrationBuilder.DropTable(
                name: "WeightCol");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrainModelId",
                table: "Weight",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<float[,]>(
                name: "WeightData",
                table: "Weight",
                type: "real[]",
                nullable: false,
                defaultValue: new float[0]);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrainModelId",
                table: "Bias",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Bias_BrainModels_BrainModelId",
                table: "Bias",
                column: "BrainModelId",
                principalTable: "BrainModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weight_BrainModels_BrainModelId",
                table: "Weight",
                column: "BrainModelId",
                principalTable: "BrainModels",
                principalColumn: "Id");
        }
    }
}
