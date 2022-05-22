using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Enfermedad
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Decripción")]
        public string Descripcion { get; set; }
    }
}
