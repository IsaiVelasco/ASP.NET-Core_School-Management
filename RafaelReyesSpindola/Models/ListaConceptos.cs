using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models
{
    public class ListaConceptos
    {
        [Display(Name = "Pago")]
        public int PagoID { get; set; }
        [Display(Name = "Concepto de Pago")]
        public int ConceptoPagoID { get; set; }

        public Pago Pago { get; set; }
        public ConceptoPago ConceptoPago { get; set; }
    }
}
