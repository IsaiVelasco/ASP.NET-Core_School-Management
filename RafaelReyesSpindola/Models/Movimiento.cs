using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Movimiento
    {
        public int ID { get; set; }
        [Display(Name ="Caja")]
        public int CajaID { get; set; }
        public Caja Caja { get; set; }
        [Display(Name = "Acción")]
        public string Accion { get; set; } //Ingreso | Egreso
        public decimal Monto { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Tipo de Movimiento")]
        public string TipoMovimiento { get; set; } // Pago/Abono | Deposito | Retiro

    }
}
