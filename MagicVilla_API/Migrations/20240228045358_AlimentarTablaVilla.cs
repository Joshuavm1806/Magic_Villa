using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Amplia Villa para pasarla bien", new DateTime(2024, 2, 27, 22, 53, 55, 350, DateTimeKind.Local).AddTicks(9742), new DateTime(2024, 2, 27, 22, 53, 55, 350, DateTimeKind.Local).AddTicks(9728), "", 120, "Villa deluxe", 4, 150.0 },
                    { 2, "", "Amplia Villa con vista al mar", new DateTime(2024, 2, 27, 22, 53, 55, 350, DateTimeKind.Local).AddTicks(9751), new DateTime(2024, 2, 27, 22, 53, 55, 350, DateTimeKind.Local).AddTicks(9750), "", 150, "Villa premium", 8, 350.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
