using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Printer
    {
        public int ID { get; set; }
        [Display(Name = "Nombre de la Ticketera")]
        public string Nombre { get; set; }
    }
}
