using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Inscripcion
    {
        public int ID { get; set; }
        [DataType(DataType.Date), 
            Display(Name = "Fecha de Inscripcion"), 
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",             
            ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "*Seleccione una fecha")]
        public DateTime FechaInscripcion { get; set; }
        //public string Estado { get; set; } //Aprobado, en curso, reporbado
        [Display(Name = "Estudiante")]
        [Required(ErrorMessage = "*Seleccione un Estudiante")]
        public int EstudianteID { get; set; }
        public Estudiante Estudiante { get; set; }

        [Display(Name = "Ciclo Escolar")]
        public int CicloEscolarID { get; set; }
        public CicloEscolar CicloEscolar { get; set; }

        [Display(Name = "Grado")]
        public int GradoID { get; set; }
        public Grado Grado { get; set; }

        [Display(Name = "Grupo")]
        [Required]
        public int GrupoID { get; set; }
        public Grupo Grupo { get; set; }
        public string Folio { get; set; }
        public string CrearMatricula(int ID)
        {
            string formatID = "";
            int x = ID.ToString().Length;
            if (x < 4)
            {
                for (int i = 0; i < (4 - x); i++)
                {
                    formatID += "0";
                }
            }
            return formatID + ID;
        }

    }
}
