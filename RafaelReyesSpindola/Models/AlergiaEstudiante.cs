using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models
{
    public class AlergiaEstudiante
    {
        public int EstudianteID { get; set; }
        public int AlergiaID { get; set; }

        public Estudiante Estudiante { get; set; }
        public Alergia Alergia { get; set; }

    }
}
