using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models
{
    public class DocenteImparteMateria
    {
        public int DocenteID { get; set; }
        public int MateriaID { get; set; }

        public Docente Docente { get; set; }
        public Materia Materia { get; set; }
    }
}
