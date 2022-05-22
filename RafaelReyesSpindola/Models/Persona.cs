using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Persona
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="*El campo Nombre(s) es obligatorio")]
        [StringLength(60, MinimumLength = 2)]
        [Display(Name = "Nombre(s)")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "*El campo Apellido Paterno es obligatorio")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "*El campo Edad es obligatorio")]
        public int Edad { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "*Seleccione una fecha")]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno;
            }
        }
        public string NombreMatricula
        {
            get
            {
                return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno + " - " + Matricula;
            }
        }
        public string CrearMatricula(string TipoPersona, int ID)
        {
            DateTime date = DateTime.Today;
            int year = date.Year % 100;
            string formatID = "";
            int x = ID.ToString().Length;
            if (x < 4)
            {
                for (int i = 0; i < (4-x); i++)
                {
                    formatID += "0";
                }
            }
            return year + TipoPersona + formatID+ID;
        }
    }
}
