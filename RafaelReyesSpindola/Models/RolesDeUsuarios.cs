using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models
{
    public class RolesDeUsuarios
    {
        public int RolID { get; set; }
        public int UsuarioID { get; set; }

        public Rol Rol { get; set; }
        public Usuario Usuario { get; set; }
    }
}
