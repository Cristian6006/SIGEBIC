using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBIC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Multas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrestamoId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoPorDia = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DiasRetraso = table.Column<int>(type: "integer", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Pagada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FechaPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Multas_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Multas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Multas_PrestamoId",
                table: "Multas",
                column: "PrestamoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multas_UsuarioId_Pagada",
                table: "Multas",
                columns: new[] { "UsuarioId", "Pagada" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Multas");
        }
    }
}
