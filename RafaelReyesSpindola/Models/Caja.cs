using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Caja
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        [Display(Name = "Monto de efectivo")]
        public decimal MontoEfectivo { get; set; }

        [Display(Name = "Usuario")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
