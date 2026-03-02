using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluationTemplateSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_evaluation_checklist_template_template_id",
                table: "evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_evaluation_answer_question_QuestionId",
                table: "evaluation_answer");

            migrationBuilder.AlterColumn<Guid>(
                name: "template_id",
                table: "evaluation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<decimal>(
                name: "confidence",
                table: "evaluation",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "template_description",
                table: "evaluation",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "template_name",
                table: "evaluation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_evaluation_checklist_template_template_id",
                table: "evaluation",
                column: "template_id",
                principalTable: "checklist_template",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_evaluation_checklist_template_template_id",
                table: "evaluation");

            migrationBuilder.DropColumn(
                name: "template_description",
                table: "evaluation");

            migrationBuilder.DropColumn(
                name: "template_name",
                table: "evaluation");

            migrationBuilder.AlterColumn<Guid>(
                name: "template_id",
                table: "evaluation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "confidence",
                table: "evaluation",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AddForeignKey(
                name: "FK_evaluation_checklist_template_template_id",
                table: "evaluation",
                column: "template_id",
                principalTable: "checklist_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_evaluation_answer_question_QuestionId",
                table: "evaluation_answer",
                column: "QuestionId",
                principalTable: "question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
