using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionImpact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question_impact",
                columns: table => new
                {
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contract_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    impact_value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_impact", x => new { x.question_id, x.contract_model_id });
                    table.ForeignKey(
                        name: "FK_question_impact_contract_model_contract_model_id",
                        column: x => x.contract_model_id,
                        principalTable: "contract_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_question_impact_question_question_id",
                        column: x => x.question_id,
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_question_impact_contract_model_id",
                table: "question_impact",
                column: "contract_model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_impact");
        }
    }
}

