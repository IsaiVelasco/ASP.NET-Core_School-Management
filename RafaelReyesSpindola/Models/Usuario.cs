using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Usuario:Persona
    {
        [Required(ErrorMessage = "*El campo Nombre de Usuario es obligatorio")]
        [Display(Name ="Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "*El campo Correo es obligatorio")]
        public string Correo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "*El campo Contraseña es obligatorio")]
        public string Password { get; set; }
        public string Sal { get; set; }

        public ICollection<RolesDeUsuarios> RolesUsuario { get; set; }
    }
}
