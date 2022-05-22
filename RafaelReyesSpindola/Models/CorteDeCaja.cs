using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class CorteDeCaja
    {
        public int ID { get; set; }
        [Display(Name = "Caja")]
        public int CajaID { get; set; }
        public Caja Caja { get; set; }
        [Display(Name = "Usuario")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        [Required(ErrorMessage = "*Seleccione una fecha")]
        public DateTime Fecha { get; set; }
        [Required]
        public decimal Contado { get; set; }
        [Required]
        public decimal Calculado { get; set; }
        [Required]
        public decimal Diferencia { get; set; }

        [Display(Name = "$1000")]
        public int B1000 { get; set; }
        [Display(Name = "$500")]
        public int B500 { get; set; }
        [Display(Name = "$200")]
        public int B200 { get; set; }
        [Display(Name = "$100")]
        public int B100 { get; set; }
        [Display(Name = "$50")]
        public int B50 { get; set; }
        [Display(Name = "$20")]
        public int B20 { get; set; }
        [Display(Name = "$10")]
        public int M10 { get; set; }
        [Display(Name = "$5")]
        public int M5 { get; set; }
        [Display(Name = "$2")]
        public int M2 { get; set; }
        [Display(Name = "$1")]
        public int M1 { get; set; }
        [Display(Name = "$0.50")]
        public int C50 { get; set; }
        [Display(Name = "$0.20")]
        public int C20 { get; set; }
        [Display(Name = "$0.10")]
        public int C10 { get; set; }


    }
}
