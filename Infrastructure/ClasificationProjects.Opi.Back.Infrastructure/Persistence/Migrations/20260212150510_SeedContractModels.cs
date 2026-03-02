using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedContractModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "contract_model",
                columns: new[] { "id", "code", "display_name" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "FIXED_PRICE", "Alcance y Precio Fijo" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "TURNKEY", "Llave en Mano (Turnkey)" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "TIME_AND_MATERIALS", "Time & Materials (T&M)" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "STAFF_AUGMENTATION", "Staff Augmentation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "contract_model",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "contract_model",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "contract_model",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "contract_model",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));
        }
    }
}

