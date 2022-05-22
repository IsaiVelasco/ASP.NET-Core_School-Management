using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RafaelReyesSpindola.Models
{
    public class Rol
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("ID")]
        public int ID { get; set; }
        public string Descripcion { get; set; }

    }
}
