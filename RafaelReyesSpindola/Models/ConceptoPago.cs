using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class ConceptoPago
    {
        public int ID { get; set; }
        
        [Required]
        public string Nombre { get; set; }
        [Required]
        public decimal Tarifa { get; set; }

        [Display(Name = "Periodo de pago")]
        public string PeriodoPago { get; set; }
        public ICollection<ListaConceptos> ListaConceptos { get; set; }        
    }
}
