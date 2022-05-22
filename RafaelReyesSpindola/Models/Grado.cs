using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{    
    public class Grado
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
        public ICollection<Materia> Materias { get; set; }

        public ICollection<Grupo> Grupos { get; set; }
    }
}
