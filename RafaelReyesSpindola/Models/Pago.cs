using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace RafaelReyesSpindola.Models
{
    public class Pago
    {
        public int ID { get; set; }

        [Display(Name = "Estudiante")]
        public int EstudianteID { get; set; }
        public Estudiante Estudiante { get; set; }

        [DataType(DataType.Date), Display(Name = "Fecha Correspondiente"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaPago { get; set; }

        [DataType(DataType.Currency)]
        public decimal Recargo { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPagar { get; set; }
        [DataType(DataType.Currency)]
        public decimal MontoPagado { get; set; }
        [DataType(DataType.Currency)]
        public decimal MontoRestante { get; set; }
               
        public string Folio { get; set; }
        public ICollection<ListaConceptos> ListaConceptos { get; set; }

        public string CrearMatricula(int ID)
        {            
            string formatID = "";
            int x = ID.ToString().Length;
            if (x < 4)
            {
                for (int i = 0; i < (4 - x); i++)
                {
                    formatID += "0";
                }
            }
            return formatID + ID;
        }
    }
}
