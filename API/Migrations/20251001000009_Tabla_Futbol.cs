using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutbolAPI.Migrations
{
    /// <inheritdoc />
    public partial class Tabla_Futbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canchas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCancha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCancha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canchas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEquipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Responsable = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Fechas_y_horas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCancha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraCancha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    EquipoRivalId = table.Column<int>(type: "int", nullable: true),
                    CanchaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fechas_y_horas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Fechas_y_horas_Canchas_CanchaId",
                        column: x => x.CanchaId,
                        principalTable: "Canchas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fechas_y_horas_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fechas_y_horas_Equipos_EquipoRivalId",
                        column: x => x.EquipoRivalId,
                        principalTable: "Equipos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fechas_y_horas_CanchaId",
                table: "Fechas_y_horas",
                column: "CanchaId");

            migrationBuilder.CreateIndex(
                name: "IX_Fechas_y_horas_EquipoId",
                table: "Fechas_y_horas",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fechas_y_horas_EquipoRivalId",
                table: "Fechas_y_horas",
                column: "EquipoRivalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fechas_y_horas");

            migrationBuilder.DropTable(
                name: "Canchas");

            migrationBuilder.DropTable(
                name: "Equipos");
        }
    }
}
