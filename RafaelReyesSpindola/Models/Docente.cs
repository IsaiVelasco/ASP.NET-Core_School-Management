using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Docente : Persona
    {        
        [Display(Name = "Profesión")]
        public string Profesion {get; set;}

        public ICollection<DocenteImparteMateria> DocenteImparteMaterias { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}
