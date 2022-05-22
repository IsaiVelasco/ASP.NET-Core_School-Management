using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RafaelReyesSpindola.Models
{
    public enum PeriodoEvaluacion
    {
        Primero, Segundo, Tercero
    }
    public class Calificacion
    {
        public int ID { get; set; }
        [Display(Name = "Docente")]
        public int DocenteID { get; set; }
        public Docente Docente { get; set; }

        [Display(Name = "Materia")]
        public int MateriaID { get; set; }
        public Materia Materia { get; set; }

        [Display(Name = "Estudiante")]
        public int EstudianteID { get; set; }
        public Estudiante Estudiante { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Display(Name = "Periodo de Evaluación")]
        public PeriodoEvaluacion PeriodoEvaliacion { get; set; }
        [Display(Name = "Ciclo Escolar")]
        public CicloEscolar CicloEscolar { get; set; }
        
        
    }
}
