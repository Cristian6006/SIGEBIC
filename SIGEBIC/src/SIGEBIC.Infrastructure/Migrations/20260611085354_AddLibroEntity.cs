using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBIC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLibroEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ISBN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Editorial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AnoPublicacion = table.Column<int>(type: "integer", nullable: false),
                    Genero = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    CantidadTotal = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CantidadDisponible = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.CheckConstraint("CK_Libros_CantidadDisponible", "\"CantidadDisponible\" >= 0 AND \"CantidadDisponible\" <= \"CantidadTotal\"");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
