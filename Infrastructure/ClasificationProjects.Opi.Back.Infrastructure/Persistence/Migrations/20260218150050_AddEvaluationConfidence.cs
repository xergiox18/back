using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluationConfidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "confidence",
                table: "evaluation",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confidence",
                table: "evaluation");
        }
    }
}
