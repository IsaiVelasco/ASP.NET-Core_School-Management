﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class PagoIndexData
    {
        public IEnumerable<Pago> Pagos { get; set; }
        public IEnumerable<ConceptoPago> ConceptosPago { get; set; }
    }
}
