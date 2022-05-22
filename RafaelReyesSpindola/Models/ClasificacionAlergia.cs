using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class ClasificacionAlergia
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nombre de la clasificación")]
        public string NombreClasificacionAlergia { get; set; }

        public ICollection<Alergia> Alergias { get; set; }
    }
}
