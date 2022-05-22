using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class ChangePassword
    {
        [Required]
        public int ID { get; set; }
        [Required(ErrorMessage = "*Escriba su nombre de usuario")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "*Escriba contraseña")]
        [Display(Name = "Contaseña")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "*Este campo es obligatorio")]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*Este campo es obligatorio")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarPassword { get; set; }
    }
}
