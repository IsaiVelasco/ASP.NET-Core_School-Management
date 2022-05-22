using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{    
    public class Tutor : Persona
    {        
        [Display(Name = "Escolaridad")]
        public int EscolaridadID { get; set; }
        public Escolaridad Escolaridad { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Ocupación")]
        public string Ocupacion { get; set; }
        [Display(Name = "Teléfono Celular")]
        public string TelefonoCelular { get; set; }
        [Display(Name = "Teléfono de Casa")]
        public string TelefonoCasa { get; set; }
        [Display(Name = "Teléfono de Trabajo")]
        public string TelefonoTrabajo { get; set; }
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }
        public string Facebook { get; set; }

        public ICollection<Pago> Pagos { get; set; }
        public ICollection<Estudiante> Estudiantes { get; set; }


       
    }
}