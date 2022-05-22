using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models
{
    public class EnfermedadEstudiante
    {
        public int EstudianteID { get; set; }
        public int EnfermedadID { get; set; }

        public Estudiante Estudiante { get; set; }
        public Enfermedad Enfermedad { get; set; }
        public string DescripcionTratamiento { get; set; }
    }
}
