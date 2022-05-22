using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class CajaMovimientos
    {
        public IEnumerable<Caja> Cajas { get; set; }
        public IEnumerable<Movimiento> Movimientos { get; set; }
        public Caja Caja { get; set; }
        public Usuario Usuario { get; set; }
        public Movimiento Movimiento { get; set; }

        public decimal TotalCaja { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Diferencia { get; set; }
    }
}
