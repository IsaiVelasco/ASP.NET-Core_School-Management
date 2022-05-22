using RafaelReyesSpindola.Models;
using System;
using System.Linq;

namespace RafaelReyesSpindola.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Usuario.Any())
            {
                return;   // DB has been seeded
            }
            var roles = new Rol[]
            {
                new Rol{ID=1, Descripcion = "Cajero"},
                new Rol{ID=2, Descripcion = "Director"},
                new Rol{ID=3, Descripcion = "Administrador"},                    
            };
            foreach (Rol r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            var usuarios = new Usuario[]
            {
            new Usuario{NombreUsuario = "Admin", Correo = "email@email.com", Password = "oFx/G84f5IrkxcWHVyYTl9DopYoYl9SMp0SFNwZ6oq8=", Sal ="RLWcGYI1zOBCYq/DA1otgw==",
                Nombre="Usuario", ApellidoPaterno="No Asig.",
                ApellidoMaterno = "No Asig.", Edad = 18, FechaNacimiento=DateTime.Parse("02/04/2002 12:00:00 a. m."),
                Matricula = "20U0001"},
            };
            foreach (Usuario u in usuarios)
            {
                context.Usuario.Add(u);
            }
            context.SaveChanges();

            var rolUsuario = new RolesDeUsuarios[]
            {
                new RolesDeUsuarios{RolID = 3, UsuarioID = 1}
            };
            foreach (RolesDeUsuarios u in rolUsuario)
            {
                context.RolesDeUsuarios.Add(u);
            }
            context.SaveChanges();

            var clasificacionesAlergias = new ClasificacionAlergia[]
            {                                              
            new ClasificacionAlergia{NombreClasificacionAlergia = "Otras alergias"},
            new ClasificacionAlergia{NombreClasificacionAlergia = "A medicamentos"},
            new ClasificacionAlergia{NombreClasificacionAlergia = "Alimentaria"},
            new ClasificacionAlergia{NombreClasificacionAlergia = "Cutanea"},
            new ClasificacionAlergia{NombreClasificacionAlergia = "Respiratoria"},
            };
            foreach (ClasificacionAlergia ca in clasificacionesAlergias)
            {
                context.ClasificacionesAlergias.Add(ca);
            }
            context.SaveChanges();

            var tiposSangre = new TipoSangre[]
            {
                new TipoSangre{NombreTipo="O negativo"},
                new TipoSangre{NombreTipo="O positivo"},
                new TipoSangre{NombreTipo="A negativo"},
                new TipoSangre{NombreTipo="A positivo"},
                new TipoSangre{NombreTipo="B negativo"},
                new TipoSangre{NombreTipo="B positivo"},
                new TipoSangre{NombreTipo="AB negativo"},
                new TipoSangre{NombreTipo="AB positivo"},
            };
            foreach (TipoSangre ts in tiposSangre)
            {
                context.TiposSangre.Add(ts);
            }
            context.SaveChanges();

            var escolaridades = new Escolaridad[]
            {
            new Escolaridad{Nombre = "Postgrado"},
            new Escolaridad{Nombre = "Superior"},
            new Escolaridad{Nombre = "Media Superior"},
            new Escolaridad{Nombre = "Secundaria"},
            new Escolaridad{Nombre = "Primaria"},
            };
            foreach (Escolaridad e in escolaridades)
            {
                context.Escolaridades.Add(e);
            }
            context.SaveChanges();

            var grados = new Grado[]
            {
            new Grado{Nombre="Sexto"},
            new Grado{Nombre="Quinto"},
            new Grado{Nombre="Cuarto"},
            new Grado{Nombre="Tercero"},
            new Grado{Nombre="Segundo"},
            new Grado{Nombre="Primero"},
            };
            foreach (Grado g in grados)
            {
                context.Grados.Add(g);
            }
            context.SaveChanges();
        }
    }
}