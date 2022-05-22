using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public class Horario
    {
        public int ID { get; set; }
        [Display(Name = "Grupo")]
        public int GrupoID { get; set; }
        public Grupo Grupo { get; set; }

        public string GradoYGrupo
        {
            get
            {
                if (Grupo != null)
                {
                    return Grupo.Grado.Nombre + " " + Grupo.NombreGrupo;
                }
                else
                {
                    return "ID Grupo: " + GrupoID;
                }
            }
        }
        public ICollection<FilaHorario> FilasHorarios { get; set; }
    }
}
