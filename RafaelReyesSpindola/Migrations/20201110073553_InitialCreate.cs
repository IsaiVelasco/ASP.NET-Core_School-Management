using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CicloEscolar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CicloEscolar", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClasificacionAlergia",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreClasificacionAlergia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasificacionAlergia", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoPago",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarifa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodoPago = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoPago", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Enfermedad",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enfermedad", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Escolaridad",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolaridad", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EscuelaProcedencia",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscuelaProcedencia", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Grado",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grado", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoMateria",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMateria", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoSangre",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSangre", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradoID = table.Column<int>(type: "int", nullable: false),
                    NombreGrupo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Grupo_Grado_GradoID",
                        column: x => x.GradoID,
                        principalTable: "Grado",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoMateriaID = table.Column<int>(type: "int", nullable: false),
                    GradoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Materia_Grado_GradoID",
                        column: x => x.GradoID,
                        principalTable: "Grado",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materia_TipoMateria_TipoMateriaID",
                        column: x => x.TipoMateriaID,
                        principalTable: "TipoMateria",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EscuelaProcedenciaID = table.Column<int>(type: "int", nullable: true),
                    PromedioProcedencia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoSangreID = table.Column<int>(type: "int", nullable: true),
                    TutorID = table.Column<int>(type: "int", nullable: true),
                    Enfermedades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EscolaridadID = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ocupacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoCelular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoCasa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoTrabajo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Persona_Escolaridad_EscolaridadID",
                        column: x => x.EscolaridadID,
                        principalTable: "Escolaridad",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persona_EscuelaProcedencia_EscuelaProcedenciaID",
                        column: x => x.EscuelaProcedenciaID,
                        principalTable: "EscuelaProcedencia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persona_Persona_TutorID",
                        column: x => x.TutorID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Persona_TipoSangre_TipoSangreID",
                        column: x => x.TipoSangreID,
                        principalTable: "TipoSangre",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilaHorario",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradoID = table.Column<int>(type: "int", nullable: true),
                    HoraEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaUno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaDos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaTres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaCuatro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaCinco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaSeis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaSiete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilaHorario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FilaHorario_Grado_GradoID",
                        column: x => x.GradoID,
                        principalTable: "Grado",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FilaHorario_Grupo_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alergia",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClasificacionAlergiaID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstudianteID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Alergia_ClasificacionAlergia_ClasificacionAlergiaID",
                        column: x => x.ClasificacionAlergiaID,
                        principalTable: "ClasificacionAlergia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alergia_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calificacion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocenteID = table.Column<int>(type: "int", nullable: false),
                    MateriaID = table.Column<int>(type: "int", nullable: false),
                    EstudianteID = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodoEvaliacion = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificacion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Calificacion_CicloEscolar_CicloEscolarID",
                        column: x => x.CicloEscolarID,
                        principalTable: "CicloEscolar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificacion_Materia_MateriaID",
                        column: x => x.MateriaID,
                        principalTable: "Materia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificacion_Persona_DocenteID",
                        column: x => x.DocenteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificacion_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DocenteImparteMateria",
                columns: table => new
                {
                    DocenteID = table.Column<int>(type: "int", nullable: false),
                    MateriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteImparteMateria", x => new { x.DocenteID, x.MateriaID });
                    table.ForeignKey(
                        name: "FK_DocenteImparteMateria_Materia_MateriaID",
                        column: x => x.MateriaID,
                        principalTable: "Materia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteImparteMateria_Persona_DocenteID",
                        column: x => x.DocenteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnfermedadEstudiante",
                columns: table => new
                {
                    EstudianteID = table.Column<int>(type: "int", nullable: false),
                    EnfermedadID = table.Column<int>(type: "int", nullable: false),
                    DescripcionTratamiento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnfermedadEstudiante", x => new { x.EstudianteID, x.EnfermedadID });
                    table.ForeignKey(
                        name: "FK_EnfermedadEstudiante_Enfermedad_EnfermedadID",
                        column: x => x.EnfermedadID,
                        principalTable: "Enfermedad",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnfermedadEstudiante_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscripcion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstudianteID = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarID = table.Column<int>(type: "int", nullable: false),
                    GradoID = table.Column<int>(type: "int", nullable: false),
                    GrupoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripcion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Inscripcion_CicloEscolar_CicloEscolarID",
                        column: x => x.CicloEscolarID,
                        principalTable: "CicloEscolar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Grado_GradoID",
                        column: x => x.GradoID,
                        principalTable: "Grado",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Grupo_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Recargo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoRestante = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstudianteID = table.Column<int>(type: "int", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pago_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pago_Persona_TutorID",
                        column: x => x.TutorID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlergiaEstudiante",
                columns: table => new
                {
                    EstudianteID = table.Column<int>(type: "int", nullable: false),
                    AlergiaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlergiaEstudiante", x => new { x.EstudianteID, x.AlergiaID });
                    table.ForeignKey(
                        name: "FK_AlergiaEstudiante_Alergia_AlergiaID",
                        column: x => x.AlergiaID,
                        principalTable: "Alergia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlergiaEstudiante_Persona_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaConceptos",
                columns: table => new
                {
                    PagoID = table.Column<int>(type: "int", nullable: false),
                    ConceptoPagoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaConceptos", x => new { x.PagoID, x.ConceptoPagoID });
                    table.ForeignKey(
                        name: "FK_ListaConceptos_ConceptoPago_ConceptoPagoID",
                        column: x => x.ConceptoPagoID,
                        principalTable: "ConceptoPago",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaConceptos_Pago_PagoID",
                        column: x => x.PagoID,
                        principalTable: "Pago",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alergia_ClasificacionAlergiaID",
                table: "Alergia",
                column: "ClasificacionAlergiaID");

            migrationBuilder.CreateIndex(
                name: "IX_Alergia_EstudianteID",
                table: "Alergia",
                column: "EstudianteID");

            migrationBuilder.CreateIndex(
                name: "IX_AlergiaEstudiante_AlergiaID",
                table: "AlergiaEstudiante",
                column: "AlergiaID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_CicloEscolarID",
                table: "Calificacion",
                column: "CicloEscolarID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_DocenteID",
                table: "Calificacion",
                column: "DocenteID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_EstudianteID",
                table: "Calificacion",
                column: "EstudianteID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_MateriaID",
                table: "Calificacion",
                column: "MateriaID");

            migrationBuilder.CreateIndex(
                name: "IX_DocenteImparteMateria_MateriaID",
                table: "DocenteImparteMateria",
                column: "MateriaID");

            migrationBuilder.CreateIndex(
                name: "IX_EnfermedadEstudiante_EnfermedadID",
                table: "EnfermedadEstudiante",
                column: "EnfermedadID");

            migrationBuilder.CreateIndex(
                name: "IX_FilaHorario_GradoID",
                table: "FilaHorario",
                column: "GradoID");

            migrationBuilder.CreateIndex(
                name: "IX_FilaHorario_GrupoID",
                table: "FilaHorario",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_GradoID",
                table: "Grupo",
                column: "GradoID");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_CicloEscolarID",
                table: "Inscripcion",
                column: "CicloEscolarID");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_EstudianteID",
                table: "Inscripcion",
                column: "EstudianteID");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_GradoID",
                table: "Inscripcion",
                column: "GradoID");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_GrupoID",
                table: "Inscripcion",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_ListaConceptos_ConceptoPagoID",
                table: "ListaConceptos",
                column: "ConceptoPagoID");

            migrationBuilder.CreateIndex(
                name: "IX_Materia_GradoID",
                table: "Materia",
                column: "GradoID");

            migrationBuilder.CreateIndex(
                name: "IX_Materia_TipoMateriaID",
                table: "Materia",
                column: "TipoMateriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_EstudianteID",
                table: "Pago",
                column: "EstudianteID");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_TutorID",
                table: "Pago",
                column: "TutorID");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_EscolaridadID",
                table: "Persona",
                column: "EscolaridadID");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_EscuelaProcedenciaID",
                table: "Persona",
                column: "EscuelaProcedenciaID");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_TipoSangreID",
                table: "Persona",
                column: "TipoSangreID");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_TutorID",
                table: "Persona",
                column: "TutorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlergiaEstudiante");

            migrationBuilder.DropTable(
                name: "Calificacion");

            migrationBuilder.DropTable(
                name: "DocenteImparteMateria");

            migrationBuilder.DropTable(
                name: "EnfermedadEstudiante");

            migrationBuilder.DropTable(
                name: "FilaHorario");

            migrationBuilder.DropTable(
                name: "Inscripcion");

            migrationBuilder.DropTable(
                name: "ListaConceptos");

            migrationBuilder.DropTable(
                name: "Alergia");

            migrationBuilder.DropTable(
                name: "Materia");

            migrationBuilder.DropTable(
                name: "Enfermedad");

            migrationBuilder.DropTable(
                name: "CicloEscolar");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropTable(
                name: "ConceptoPago");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "ClasificacionAlergia");

            migrationBuilder.DropTable(
                name: "TipoMateria");

            migrationBuilder.DropTable(
                name: "Grado");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Escolaridad");

            migrationBuilder.DropTable(
                name: "EscuelaProcedencia");

            migrationBuilder.DropTable(
                name: "TipoSangre");
        }
    }
}
