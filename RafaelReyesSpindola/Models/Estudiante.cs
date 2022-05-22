using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RafaelReyesSpindola.Models
{
    public class Estudiante : Persona
    {
        [Display(Name = "Escuela de Procedencia")]
        public int EscuelaProcedenciaID { get; set; }
        [Display(Name = "Escuela de Procedencia")]
        public EscuelaProcedencia EscuelaProcedencia { get; set; }

        [Display(Name = "Promedio de Procedencia")]
        public decimal PromedioProcedencia { get; set; }
        [Display(Name = "Tipo de Sangre")]
        public int TipoSangreID { get; set; }

        [Display(Name = "Tipo de Sangre")]
        public TipoSangre TipoSangre { get; set; }               

        [Display(Name = "Tutor")]
        public int TutorID { get; set; }
        public Tutor Tutor { get; set; }

        [Display(Name = "Parentesco del Tutor")]
        [Required(ErrorMessage = "*El campo Parentesco es obligatorio")]
        public string ParentescoTutor { get; set; }

        [DataType(DataType.MultilineText)]
        public string Enfermedades { get; set; }

        [Display(Name = "Alergias")]
        public ICollection<Alergia> AlergiasEstudiante { get; set; }        
        public ICollection<Inscripcion> Inscripciones { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
        public ICollection<Pago> Pagos { get; set; }      
        public ICollection<EnfermedadEstudiante> EnfermedadesEstudiante { get; set; }

        public List<string> PagosAtrasdos(ICollection<Pago> Pagos, ICollection<Inscripcion> Inscripciones)
        {
            List<string> listaMensajes = new List<string>();
            if ( Inscripciones.Count != 0)
            {
                Inscripcion inscripcion = Inscripciones.Last();
                List<Pago> PagosCiclo = new List<Pago>();
                DateTime date = DateTime.Today;
                DateTime fechaInscripcion = inscripcion.FechaInscripcion;
                DateTime inicioCiclo = inscripcion.CicloEscolar.FechaInicio;
                DateTime finCiclo = inscripcion.CicloEscolar.FechaFin;
                //Selecciona todos los pagos del último ciclo
                foreach (Pago pago in Pagos)
                {
                    if (pago.FechaPago >= inicioCiclo & pago.FechaPago <= finCiclo)
                    {
                        PagosCiclo.Add(pago);
                    }
                }
                int indice = finCiclo.Month;
                if (date < finCiclo)
                {
                    indice = date.Month;
                }
                int iteraciones = (13 - inicioCiclo.Month) + indice;
                int mesRango = inicioCiclo.Month;
                Boolean comenzarCargos = true; 
                if(fechaInscripcion > inicioCiclo & fechaInscripcion.Month != inicioCiclo.Month)
                {
                    comenzarCargos = false; //Este if se realiza por si un alumno ingresa a mitad de ciclo
                }

                for (int i = 0; i < iteraciones; i++)
                {
                    if(mesRango == fechaInscripcion.Month & comenzarCargos !=true)
                    {
                        comenzarCargos = true;
                    }
                    DateTime fechaAdeudada = inicioCiclo.AddMonths(i);
                    if (!pagoParaMes(mesRango, PagosCiclo))
                    {

                        //Agrega que debe un pago para este mes
                        CultureInfo ci = new CultureInfo("es-ES");

                        string FechaFormateada = fechaAdeudada.ToString("MMMM", ci) + " / " + fechaAdeudada.ToString("yyyy");
                        if (comenzarCargos & comienzaCiclo(date, inicioCiclo))
                        {
                            listaMensajes.Add(FechaFormateada);
                        }
                        

                    }
                    mesRango++;
                    if (mesRango > 12) mesRango = 1;
                }
            }

            return listaMensajes;
        }

        private Boolean pagoParaMes(int esteMes, List<Pago> pagosCiclo)
        {
            Boolean hayPago = false;
            for (int i = 1; i < 32; i++)
            {
                foreach (Pago pago in pagosCiclo)
                {
                    if (pago.FechaPago.Month == esteMes && pago.FechaPago.Day == i)
                    {
                        foreach (var concepto in pago.ListaConceptos)
                        {
                            if(concepto.ConceptoPago.PeriodoPago == "Mensual")
                            {
                                hayPago = true;
                            }
                        }
                    }
                }
            }
            return hayPago;
        }
        private Boolean comienzaCiclo(DateTime date, DateTime inicioCiclo)
        {
            Boolean comienza = true;
            if (date < inicioCiclo & date.Month != inicioCiclo.Month)
            {
                comienza = false;
            }
            return comienza;
            
        }
    }
}
