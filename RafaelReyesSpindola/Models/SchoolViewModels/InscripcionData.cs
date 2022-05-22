using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class InscripcionData
    {
        public Inscripcion Inscripcion { get; set; }
        public Tutor Tutor { get; set; }
        public Estudiante Estudiante { get; set; }
        public Horario Horario { get; set; }
    }
}
