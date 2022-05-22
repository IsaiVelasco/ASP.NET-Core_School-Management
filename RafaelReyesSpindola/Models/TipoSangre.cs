using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class TipoSangre
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string NombreTipo { get; set; }
    }
}
