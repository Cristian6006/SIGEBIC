using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBIC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrestamoEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    LibroId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaDevolucionEsperada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaDevolucionReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CantidadRenovaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamos_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialPrestamos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LibroId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrestamoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaDevolucionReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstadoFinal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DiasRetraso = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialPrestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialPrestamos_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialPrestamos_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialPrestamos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPrestamos_LibroId_FechaPrestamo",
                table: "HistorialPrestamos",
                columns: new[] { "LibroId", "FechaPrestamo" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPrestamos_PrestamoId",
                table: "HistorialPrestamos",
                column: "PrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPrestamos_UsuarioId_FechaPrestamo",
                table: "HistorialPrestamos",
                columns: new[] { "UsuarioId", "FechaPrestamo" });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_UsuarioId",
                table: "Prestamos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialPrestamos");

            migrationBuilder.DropTable(
                name: "Prestamos");
        }
    }
}
