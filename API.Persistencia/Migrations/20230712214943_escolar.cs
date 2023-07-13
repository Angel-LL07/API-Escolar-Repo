using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class escolar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCarrera = table.Column<string>(type: "text", nullable: false),
                    Siglas = table.Column<string>(type: "text", nullable: false),
                    NombreReducido = table.Column<string>(type: "text", nullable: false),
                    Reticula = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Abreviacion = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaTemino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    NoControl = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApellidoPaterno = table.Column<string>(name: "Apellido_Paterno", type: "text", nullable: false),
                    ApellidoMaterno = table.Column<string>(name: "Apellido_Materno", type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Edad = table.Column<int>(type: "integer", nullable: false),
                    Curp = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(name: "Fecha_Nacimiento", type: "timestamp with time zone", nullable: false),
                    Sexo = table.Column<char>(type: "character(1)", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.NoControl);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaveMateria = table.Column<string>(type: "text", nullable: false),
                    NombreMateria = table.Column<string>(type: "text", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: false),
                    NoUnidades = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materias_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstudiantesNoControl = table.Column<int>(type: "integer", nullable: false),
                    PeriodoId = table.Column<int>(type: "integer", nullable: false),
                    Unidad1 = table.Column<int>(type: "integer", nullable: false),
                    Unidad2 = table.Column<int>(type: "integer", nullable: false),
                    Unidad3 = table.Column<int>(type: "integer", nullable: false),
                    Unidad4 = table.Column<int>(type: "integer", nullable: false),
                    Unidad5 = table.Column<int>(type: "integer", nullable: false),
                    CalificacionFinal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Estudiantes_EstudiantesNoControl",
                        column: x => x.EstudiantesNoControl,
                        principalTable: "Estudiantes",
                        principalColumn: "NoControl",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Periodos_PeriodoId",
                        column: x => x.PeriodoId,
                        principalTable: "Periodos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_EstudiantesNoControl",
                table: "Calificaciones",
                column: "EstudiantesNoControl");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_PeriodoId",
                table: "Calificaciones",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_CarreraId",
                table: "Estudiantes",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_CarreraId",
                table: "Materias",
                column: "CarreraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Periodos");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}
