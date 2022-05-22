using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Grupo
    {
        public int ID { get; set; }

        [Display(Name = "Grado")]
        public int GradoID { get; set; }
        public Grado Grado { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Grupo")]
        public string NombreGrupo { get; set; }

        public string GradoYGrupo
        {
            get
            {
                if (Grado!=null)
                {
                    return Grado.Nombre + " " + NombreGrupo;
                }
                else
                {
                    return "No se puede mostrar el grado del grupo: " + NombreGrupo;
                }
            }
        }
        public ICollection<FilaHorario> FilasHorarios { get; set; }
    }
}
