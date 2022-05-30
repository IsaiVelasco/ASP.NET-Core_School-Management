using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using RafaelReyesSpindola.Models.SchoolViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize(Roles = "Administrador, Cajero")]
    public class PagosController : Controller
    {
        private readonly SchoolContext _context;

        public PagosController(SchoolContext context)
        {
            _context = context;
        }
        
        // GET: Pagos
        public async Task<IActionResult> Index(int? id,
            string searchAtributo,
            string searchFecha)
        {
            
            var viewModel = new PagoIndexData();
            viewModel.Pagos =  await _context.Pagos
                .Include(p => p.Estudiante)
                    .ThenInclude(e => e.Tutor)
                .Include(p => p.ListaConceptos)
                    .ThenInclude(p => p.ConceptoPago)
                .AsNoTracking()
                .OrderBy(p => p.Folio)
                .ToListAsync();
            if (!String.IsNullOrEmpty(searchFecha))
            {
                DateTime fecha = DateTime.Parse(searchFecha);
                viewModel.Pagos = viewModel.Pagos.Where(s => (s.FechaPago.Date == fecha.Date));
                ViewData["CurrentFilterFecha"] = searchFecha;
            }
            if (!String.IsNullOrEmpty(searchAtributo))
            {
                viewModel.Pagos = viewModel.Pagos.Where(s => ( // 091823 isai velasco
                    searchAtributo.Contains(s.Estudiante.Matricula) & searchAtributo.Contains(s.Estudiante.ApellidoPaterno) & searchAtributo.Contains(s.Estudiante.ApellidoMaterno))
                    || (searchAtributo.Contains(s.Estudiante.Nombre) & searchAtributo.Contains(s.Estudiante.ApellidoPaterno)
                        || searchAtributo.Contains(s.Estudiante.ApellidoPaterno) & searchAtributo.Contains(s.Estudiante.ApellidoMaterno))
                    || s.Estudiante.Nombre.Contains(searchAtributo)
                    || s.Estudiante.ApellidoPaterno.Contains(searchAtributo)
                    || s.Estudiante.ApellidoMaterno.Contains(searchAtributo)
                    || searchAtributo.Contains(s.Folio)
                    || searchAtributo.Contains(s.Estudiante.Matricula)
                    || s.Folio.Contains(searchAtributo));
                ViewData["CurrentFilter"] = searchAtributo;
            }
            
            if (id != null)
            {
                ViewData["PagoID"] = id.Value;
                Pago pago = viewModel.Pagos.Where(
                    p => p.ID == id.Value).Single();
                viewModel.ConceptosPago = pago.ListaConceptos.Select(l => l.ConceptoPago);
            }
            /*var schoolContext = _context.Pagos
                .Include(p => p.Estudiante)
                .Include(p => p.ListaConceptos)
                .ThenInclude(p => p.ConceptoPago)
                .AsNoTracking();*/
            return View(viewModel);
        }
        public async Task<ActionResult> ReciboPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new RecibosPago();
            viewModel.Pago = await _context.Pagos
                .Include(p => p.Estudiante).ThenInclude(e => e.Tutor)
                .Include(p => p.ListaConceptos)
                    .ThenInclude(p => p.ConceptoPago)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (viewModel.Pago == null)
            {
                return NotFound();
            }

            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).FirstOrDefault();
            if (usuario != null)
                viewModel.Usuario = usuario;

            DateTime today = DateTime.Now;
            string customSwitches = string.Format(" --allow {0} --footer-html {0} --footer-spacing 0",
                Url.Action("FooterPDF", "Cajas", new { area = "" }, "http"));
            return new ViewAsPdf("ReciboPDF", viewModel)
            {
                FileName = today + "-" + viewModel.Pago.Folio + ".pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Margins(15, 16, 16, 16),
                CustomSwitches = customSwitches// "--print-media-type --footer-center \"text\""
            };
        }

        [AllowAnonymous]
        public ActionResult FooterPDF()
        {
            return View();
        }

        public async Task<IActionResult> CrearTicket(int? id)
        {
            

            var pago = await _context.Pagos
                .Include(p => p.Estudiante)
                .Include(p => p.ListaConceptos)
                    .ThenInclude(p => p.ConceptoPago)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            //Creamos una instancia d ela clase CrearTicket
            CrearTicket ticket = new CrearTicket();
            //Ya podemos usar todos sus metodos
            //ticket.AbreCajon();//Para abrir el cajon de dinero.

            //De aqui en adelante pueden formar su ticket a su gusto... Les muestro un ejemplo

            //Datos de la cabecera del Ticket.
            ticket.TextoCentro("INSTITUTO RAFAEL REYES SPINDOLA");
            ticket.TextoCentro("TICKET DE PAGO - "+pago.Folio);
            ticket.TextoIzquierda("Fecha: "+pago.FechaPago.ToString());
            ticket.lineasIgual();
            ticket.TextoIzquierda("Conceptos del pago:");
            ticket.lineasGuio();
            ticket.TextoIzquierda("CONCEPTO         || TARIFA        ");
            foreach (var concepto in pago.ListaConceptos)
            {
                ticket.TextoIzquierda(concepto.ConceptoPago.Nombre+": $"+ concepto.ConceptoPago.Tarifa);
            }
            ticket.lineasGuio();
            //Sub cabecera.
            ticket.TextoIzquierda("ESTUDIANTE:");
            ticket.TextoIzquierda(pago.Estudiante.NombreMatricula);
            ticket.lineasGuio();
            ticket.TextoIzquierda("Recargo: $" + pago.Recargo);
            ticket.TextoIzquierda("Total a pagar: $"+pago.TotalPagar);
            ticket.TextoIzquierda("Monto pagado: $" + pago.MontoPagado);
            ticket.TextoIzquierda("Monto restante: $" + pago.MontoRestante);
            ticket.TextoCentro(" ");
            ticket.TextoCentro("¡GRACIAS!");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            ticket.TextoCentro(" ");
            //ticket.CortaTicket();
            var printer = await _context.Printers.ToListAsync();
            string printerName = "no-printer";
            if (printer.Count() != 0)
            {
                printerName = printer.First().Nombre;
            }
            //ticket.ImprimirTicket("POS-58-Series");//Nombre de la impresora ticketera
            ticket.ImprimirTicket(printerName);//Nombre de la impresora ticketera

            if (pago == null)
            {
                return NotFound();
            }

            return Redirect("/Pagos/Details/"+pago.ID);
        }
        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Estudiante)
                .Include(p => p.ListaConceptos)
                    .ThenInclude(p => p.ConceptoPago)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create(int? id)
        {
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {                
                var caja = _context.Caja.Where(
                        c => c.UsuarioID == int.Parse(userID))
                        .Include(c => c.Movimientos)
                        .Include(c => c.Usuario)
                        .Single();
                var selectList = new SelectList(_context.Estudiantes, "ID", "NombreMatricula");
                if (id != null)
                {
                    var estudiantes = _context.Estudiantes
                    .Include(e => e.EscuelaProcedencia)
                    .Include(e => e.TipoSangre)
                    .Include(e => e.Tutor)
                    .Include(e => e.Pagos)
                    .ThenInclude(p => p.ListaConceptos)
                        .ThenInclude(cp => cp.ConceptoPago)
                    .Include(e => e.Inscripciones)
                    .ThenInclude(c => c.CicloEscolar);
                    Estudiante estudiante = estudiantes.Where(
                            t => t.ID == id.Value).Single();
                    
                    foreach (var item in estudiante.PagosAtrasdos(estudiante.Pagos, estudiante.Inscripciones))
                    {
                        ModelState.AddModelError(string.Empty, "Retraso de colegiatura: "+item);
                    }
                    foreach (var item in selectList)
                    {
                        if (item.Value.ToString() == estudiante.ID.ToString())
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Disabled = true;
                        }
                    }
                }
                var pago = new Pago();
                pago.ListaConceptos = new List<ListaConceptos>();
                PopulateAssignedConceptData(pago);
                ViewData["EstudianteID"] = selectList;                

                return View();
            }
            catch(InvalidOperationException)
            {
                return Redirect("/Cajas/Create");
            }

            
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EstudianteID,FechaPago,Recargo,TotalPagar,MontoPagado,MontoRestante,Folio")] Pago pago, string[] selectedConceptosPago)
        {
            if (selectedConceptosPago != null)
            {
                pago.ListaConceptos = new List<ListaConceptos>();
                foreach (var conceptoPago in selectedConceptosPago)
                {
                    var ConceptoPagoToAdd = new ListaConceptos { PagoID = pago.ID, ConceptoPagoID = int.Parse(conceptoPago) };
                    pago.ListaConceptos.Add(ConceptoPagoToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                pago.ID = 0;
                _context.Add(pago);
                await _context.SaveChangesAsync();

                //CREA EL MOVIMIENTO AUTOMÁTICO
                var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                var caja = _context.Caja.Where(
                        c => c.UsuarioID == int.Parse(userID))
                        .Include(c => c.Movimientos)
                        .Include(c => c.Usuario)
                        .Single();
                Movimiento movimiento = new Movimiento();
                movimiento.CajaID = caja.ID;
                movimiento.Accion = "Ingreso";
                movimiento.Monto = pago.MontoPagado;
                movimiento.Fecha = DateTime.Now;
                movimiento.TipoMovimiento = "Pago";
                //SUMAR O RESTAR EFECTIVO A LA CAJA
                try
                {
                    if (movimiento.Accion == "Ingreso")
                    {
                        caja.MontoEfectivo = caja.MontoEfectivo + movimiento.Monto;
                    }
                    else if (movimiento.Accion == "Egreso")
                    {
                        caja.MontoEfectivo = caja.MontoEfectivo - movimiento.Monto;
                    }
                    _context.Update(caja);
                    await _context.SaveChangesAsync();
                    //Si se hizo la transaccion en la caja, se registra el movimiento
                    movimiento.ID = 0;
                    _context.Add(movimiento);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Details), "Cajas", new { ID = CajaID });
                    //return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CajaExists(caja.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //FIN DEL MOVIMIENTO AUTOMÁTICO

                pago.Folio = pago.CrearMatricula(pago.ID);
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = pago.ID });
                //return RedirectToAction(nameof(Index));
            }
            pago.Folio = pago.CrearMatricula(pago.ID);
            try
            {
                _context.Update(pago);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(pago.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            PopulateAssignedConceptData(pago);
            //Asigna la atrícula automática            
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", pago.EstudianteID);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.ListaConceptos).ThenInclude(p => p.ConceptoPago)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pago == null)
            {
                return NotFound();
            }
            PopulateAssignedConceptData(pago);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "NombreMatricula", pago.EstudianteID);
            return View(pago);
        }

        private void PopulateAssignedConceptData(Pago pago)
        {
            var allConceptosPago = _context.ConceptosPago;
            var pagoConceptosPago = new HashSet<int>(pago.ListaConceptos.Select(c => c.ConceptoPagoID));
            var viewModel = new List<AssignedConceptData>();
            foreach (var conceptoPago in allConceptosPago)
            {
                viewModel.Add(new AssignedConceptData
                {
                    ConceptoPagoID = conceptoPago.ID,
                    Nombre = conceptoPago.Nombre,
                    Tarifa = conceptoPago.Tarifa,
                    Assigned = pagoConceptosPago.Contains(conceptoPago.ID),                   
                });
            }
            ViewData["ConceptosPago"] = viewModel;
        }
        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,EstudianteID,FechaPago,Recargo,TotalPagar,MontoPagado,MontoRestante,Folio")] Pago pago)
        public async Task<IActionResult> Edit(int? id, string[] selectedConceptosPago)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagoToUpdate = await _context.Pagos
                .Include(p => p.ListaConceptos)
                    .ThenInclude(p => p.ConceptoPago)
                .FirstOrDefaultAsync(m => m.ID == id);
            var pagoToErase = pagoToUpdate.MontoPagado;
            if (await TryUpdateModelAsync<Pago>(
                pagoToUpdate,
                "",
                p => p.Estudiante, p => p.FechaPago, p => p.Recargo, p => p.TotalPagar, p => p.MontoPagado, p => p.MontoRestante))
            {               
                UpdatePagoConceptos(selectedConceptosPago, pagoToUpdate);
                try
                {
                    await _context.SaveChangesAsync();

                    //MOV AUTOMATICO
                    //CREA EL MOVIMIENTO AUTOMÁTICO
                    var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                    var caja = _context.Caja.Where(
                            c => c.UsuarioID == int.Parse(userID))
                            .Include(c => c.Movimientos)
                            .Include(c => c.Usuario)
                            .Single();
                    
                    Movimiento movimiento = new Movimiento();
                    movimiento.CajaID = caja.ID;                                        
                    movimiento.Fecha = DateTime.Now;
                   
                    if (pagoToUpdate.MontoPagado < pagoToErase)
                    {
                        movimiento.Accion = "Egreso";
                        movimiento.TipoMovimiento = "Pago Actualizado";
                        movimiento.Monto = pagoToErase - pagoToUpdate.MontoPagado;
                    }
                    else if (pagoToUpdate.MontoPagado > pagoToErase)
                    {
                        movimiento.Accion = "Ingreso";
                        movimiento.TipoMovimiento = "Abono";
                        movimiento.Monto = pagoToUpdate.MontoPagado -pagoToErase;
                    } 
                    else if (pagoToUpdate.MontoPagado == pagoToErase)
                    {
                        movimiento = null;
                    }
                        //SUMAR O RESTAR EFECTIVO A LA CAJA
                    try
                    {
                        if(movimiento != null)
                        {
                            if (movimiento.Accion == "Ingreso")
                            {
                                caja.MontoEfectivo = caja.MontoEfectivo + movimiento.Monto;
                            }
                            else if (movimiento.Accion == "Egreso")
                            {
                                caja.MontoEfectivo = caja.MontoEfectivo - movimiento.Monto;
                            }                            
                            _context.Update(caja);
                            await _context.SaveChangesAsync();
                            //Si se hizo la transaccion en la caja, se registra el movimiento
                            movimiento.ID = 0;
                            _context.Add(movimiento);
                            await _context.SaveChangesAsync();
                            //return RedirectToAction(nameof(Index));
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CajaExists(caja.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //FIN DEL MOVIMIENTO AUTOMÁTICO                    
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePagoConceptos(selectedConceptosPago, pagoToUpdate);
            PopulateAssignedConceptData(pagoToUpdate);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", pagoToUpdate.EstudianteID);
            return View(pagoToUpdate);           
        }
        private void UpdatePagoConceptos(string[] selectedConceptosPago, Pago pagoToUpdate)
        {
            if (selectedConceptosPago == null)
            {
                pagoToUpdate.ListaConceptos = new List<ListaConceptos>();
                return;
            }

            var selectedConceptosPagoHS = new HashSet<string>(selectedConceptosPago);
            var pagosConceptosPago = new HashSet<int>
                (pagoToUpdate.ListaConceptos.Select(c => c.ConceptoPago.ID));
            foreach (var conceptoPago in _context.ConceptosPago)
            {
                if (selectedConceptosPagoHS.Contains(conceptoPago.ID.ToString()))
                {
                    if (!pagosConceptosPago.Contains(conceptoPago.ID))
                    {
                        pagoToUpdate.ListaConceptos.Add(new ListaConceptos { PagoID = pagoToUpdate.ID, ConceptoPagoID = conceptoPago.ID });
                    }
                }
                else
                {

                    if (pagosConceptosPago.Contains(conceptoPago.ID))
                    {
                        ListaConceptos conceptosPagoToRemove = pagoToUpdate.ListaConceptos.FirstOrDefault(p => p.ConceptoPagoID == conceptoPago.ID);
                        _context.Remove(conceptosPagoToRemove);
                    }
                }
            }
        }
        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Estudiante)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.ID == id);
        }

        private bool CajaExists(int id)
        {
            return _context.Caja.Any(e => e.ID == id);
        }
    }
}
