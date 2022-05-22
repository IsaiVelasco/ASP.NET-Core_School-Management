using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class EscuelaProcedencia
    {
        public int ID { get; set; }

        [StringLength(100)]
        [Required]
        public string Nombre { get; set; }
    }
}
