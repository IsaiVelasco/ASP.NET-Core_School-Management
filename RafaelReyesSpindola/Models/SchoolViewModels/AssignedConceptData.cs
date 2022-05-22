using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class AssignedConceptData
    {
        public int ConceptoPagoID { get; set; }
        public string Nombre { get; set; }
        public decimal Tarifa { get; set; }
        public bool Assigned { get; set; }

    }
}
