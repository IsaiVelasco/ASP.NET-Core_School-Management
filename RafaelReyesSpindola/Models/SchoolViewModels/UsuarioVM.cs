using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "*Escriba su nombre de usuario")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "*Escriba su contraseña")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    
    }
}
