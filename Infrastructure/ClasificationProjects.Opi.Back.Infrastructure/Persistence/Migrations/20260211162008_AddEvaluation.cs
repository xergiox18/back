using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "evaluation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recommended_contract_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    client_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    evaluated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluation_checklist_template_template_id",
                        column: x => x.template_id,
                        principalTable: "checklist_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluation_contract_model_recommended_contract_model_id",
                        column: x => x.recommended_contract_model_id,
                        principalTable: "contract_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_recommended_contract_model_id",
                table: "evaluation",
                column: "recommended_contract_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_template_id",
                table: "evaluation",
                column: "template_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "evaluation");
        }
    }
}

