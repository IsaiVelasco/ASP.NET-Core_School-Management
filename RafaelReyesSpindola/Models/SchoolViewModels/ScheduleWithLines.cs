using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RafaelReyesSpindola.Models.SchoolViewModels
{
    public class ScheduleWithLines
    {
        public IEnumerable<Horario> Horarios { get; set; }
        public IEnumerable<FilaHorario> FilasHorarios { get; set; }
        public Horario Horario { get; set; }
        public FilaHorario Fila { get; set; }
    }
}
