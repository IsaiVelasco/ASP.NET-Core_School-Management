using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Materia
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de materia")]
        public int TipoMateriaID { get; set; }
        public TipoMateria TipoMateria { get; set; }
        [Display(Name = "Grado")]
        public int GradoID { get; set; }
        public Grado Grado { get; set; }

        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}
