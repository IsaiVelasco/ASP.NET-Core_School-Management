using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Alergia
    {
        public int ID { get; set; }
        [Display(Name = "Tipo de alergia")]
        public int ClasificacionAlergiaID { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        
        public ClasificacionAlergia ClasificacionAlergia { get; set; }
    }
}
