using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueOrderPerTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_question_template_id",
                table: "question");

            migrationBuilder.CreateIndex(
                name: "IX_question_template_id_order",
                table: "question",
                columns: new[] { "template_id", "order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_question_template_id_order",
                table: "question");

            migrationBuilder.CreateIndex(
                name: "IX_question_template_id",
                table: "question",
                column: "template_id");
        }
    }
}

