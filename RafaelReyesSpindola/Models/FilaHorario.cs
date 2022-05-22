using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class FilaHorario
    {
        public int ID { get; set; }

        [Display(Name = "Horario para:")]
        public int HorarioID { get; set; }
        public Horario Horario { get; set; }

        [Required]
        [DataType(DataType.Date), Display(Name = "Hora de Entrada"), DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime HoraEntrada { get; set; }

        [Required]
        [DataType(DataType.Date), Display(Name = "Hora de Salida"), DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime HoraSalida{ get; set; }

        [Display(Name = "Lunes")]
        public string DiaUno { get; set; }

        [Display(Name = "Martes")]
        public string DiaDos { get; set; }

        [Display(Name = "Miércoles")]
        public string DiaTres { get; set; }

        [Display(Name = "Jueves")]
        public string DiaCuatro { get; set; }

        [Display(Name = "Viernes")]
        public string DiaCinco { get; set; }

        [Display(Name = "Sábado")]
        public string DiaSeis { get; set; }

        [Display(Name = "Domingo")]
        public string DiaSiete { get; set; }

    }
}
