using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RafaelReyesSpindola.Models;
using Microsoft.EntityFrameworkCore;

namespace RafaelReyesSpindola.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Alergia> Alergia { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<CicloEscolar> CiclosEscolares { get; set; }
        public DbSet<ClasificacionAlergia> ClasificacionesAlergias { get; set; }
        public DbSet<ConceptoPago> ConceptosPago { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Enfermedad> Enfermedades { get; set; }
        public DbSet<Escolaridad> Escolaridades { get; set; }
        public DbSet<EscuelaProcedencia> EscuelasProcedencia { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<FilaHorario> FilaHorarios { get; set; }
        public DbSet<Grado> Grados { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<TipoSangre> TiposSangre { get; set; }
        public DbSet<Tutor> Tutores { get; set; }

        public DbSet<Rol> Roles { get; set; }

        public DbSet<AlergiaEstudiante> AlergiasEstudiantes { get; set; }
        public DbSet<DocenteImparteMateria> DocentesImpartenMaterias { get; set; }
        public DbSet<EnfermedadEstudiante> EnfermedadesEstudiantes { get; set; }
        public DbSet<ListaConceptos> ListaConceptos { get; set; }
        public DbSet<RolesDeUsuarios> RolesDeUsuarios { get; set; }

        public DbSet<Horario> Horario { get; set; }
        public DbSet<TipoMateria> TipoMateria { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Caja> Caja { get; set; }
        public DbSet<Movimiento> Movimiento { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alergia>().ToTable("Alergia");
            modelBuilder.Entity<Calificacion>().ToTable("Calificacion");
            modelBuilder.Entity<CicloEscolar>().ToTable("CicloEscolar");
            modelBuilder.Entity<ClasificacionAlergia>().ToTable("ClasificacionAlergia");
            modelBuilder.Entity<ConceptoPago>().ToTable("ConceptoPago");
            if (typeof(Estudiante).BaseType == null)
                modelBuilder.Entity<Docente>().ToTable("Docente");
            modelBuilder.Entity<Enfermedad>().ToTable("Enfermedad");
            modelBuilder.Entity<Escolaridad>().ToTable("Escolaridad");
            modelBuilder.Entity<EscuelaProcedencia>().ToTable("EscuelaProcedencia");
            if (typeof(Estudiante).BaseType == null)
                modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
            modelBuilder.Entity<FilaHorario>().ToTable("FilaHorario");
            modelBuilder.Entity<Grado>().ToTable("Grado");
            modelBuilder.Entity<Grupo>().ToTable("Grupo");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Materia>().ToTable("Materia");
            modelBuilder.Entity<Pago>().ToTable("Pago");
            modelBuilder.Entity<Persona>().ToTable("Persona");
            modelBuilder.Entity<TipoSangre>().ToTable("TipoSangre");
            if (typeof(Estudiante).BaseType == null)
                modelBuilder.Entity<Tutor>().ToTable("Tutor");

            modelBuilder.Entity<AlergiaEstudiante>().ToTable("AlergiaEstudiante");
            modelBuilder.Entity<DocenteImparteMateria>().ToTable("DocenteImparteMateria");
            modelBuilder.Entity<EnfermedadEstudiante>().ToTable("EnfermedadEstudiante");
            modelBuilder.Entity<ListaConceptos>().ToTable("ListaConceptos");
            if (typeof(Usuario).BaseType == null)
                modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Caja>().ToTable("Caja");
            modelBuilder.Entity<Movimiento>().ToTable("Movimiento");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<RolesDeUsuarios>().ToTable("RolesDeUsuarios");

            modelBuilder.Entity<AlergiaEstudiante>()
                .HasKey(c => new { c.EstudianteID, c.AlergiaID });
            modelBuilder.Entity<DocenteImparteMateria>()
                .HasKey(c => new { c.DocenteID, c.MateriaID });
            modelBuilder.Entity<EnfermedadEstudiante>()
                .HasKey(c => new { c.EstudianteID, c.EnfermedadID });
            modelBuilder.Entity<ListaConceptos>()
                .HasKey(c => new { c.PagoID, c.ConceptoPagoID });
            modelBuilder.Entity<RolesDeUsuarios>()
                .HasKey(c => new { c.RolID, c.UsuarioID });
        }
        public DbSet<RafaelReyesSpindola.Models.CorteDeCaja> CorteDeCaja { get; set; }


        
    }
}