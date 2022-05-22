using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class CicloEscolar
    {
        public int ID { get; set; }       
        [DataType(DataType.Date), Display(Name = "Fecha de Inicio"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }        
        [DataType(DataType.Date), Display(Name = "Fecha de Término"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }

        public string InicioFin
        {
            get
            {                
                return FechaInicio.ToString("yyyy") + " - " + FechaFin.ToString("yyyy");
            }
        }
        public string CicloCompleto
        {
            get
            {
                return FechaInicio.ToString("dd MMMM yyyy") + " - " + FechaFin.ToString("dd MMMM yyyy");
            }
        }
        public ICollection<Calificacion> Calificaciones { get; set; }

    }    
}
