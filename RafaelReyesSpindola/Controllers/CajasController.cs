using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using RafaelReyesSpindola.Models.SchoolViewModels;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize(Roles = "Administrador, Cajero")]
    public class CajasController : Controller
    {
        private readonly SchoolContext _context;

        public CajasController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Cajas
       
        public async Task<IActionResult> Index()
        {
            /*Busca el usuario para mostrar sólo sus cajero correspondiente*/
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID))
                .Include(m => m.RolesUsuario).ThenInclude(r => r.Rol)
                .FirstOrDefault();

            var schoolContext = _context.Caja
                        .Where(c => c.Usuario == usuario)
                        .Include(c => c.Usuario);
            
            foreach(var rol in usuario.RolesUsuario)
            {
                if (rol.Equals("Administrador"))
                {
                    schoolContext = _context.Caja.Include(c => c.Usuario);
                }
            }
            
            return View(await schoolContext.ToListAsync());
        }

        public async Task<ActionResult> ReporteCajaPDF(int? id,
            string searchString,
            string searchDesde,
            string searchHasta)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new CajaMovimientos();
            var caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            decimal i = 0, e = 0; // i = ingreso, e = egreso
            foreach (var mov in caja.Movimientos)
            {
                if (mov.Accion == "Ingreso")
                    i += mov.Monto;
                else
                    e += mov.Monto;
            }
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).FirstOrDefault();
            if (usuario != null)
                viewModel.Usuario = usuario;
            viewModel.Caja = caja;
            viewModel.Movimiento = new Movimiento();
            viewModel.TotalIngresos = i;
            viewModel.TotalEgresos = e;
            viewModel.TotalCaja = i - e;
            viewModel.Diferencia = i - e;
            if (!String.IsNullOrEmpty(searchString))
            {
                DateTime fecha = DateTime.Parse(searchString);
                caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);

                for (int it = caja.Movimientos.Count() - 1; it >= 0; it--)
                {
                    if (!(caja.Movimientos.ElementAt(it).Fecha.Date == fecha.Date))
                    {
                        caja.Movimientos.Remove(caja.Movimientos.ElementAt(it));
                    }
                }
                i = 0; e = 0; // i = ingreso, e = egreso
                foreach (var mov in caja.Movimientos)
                {
                    if (mov.Accion == "Ingreso")
                        i += mov.Monto;
                    else
                        e += mov.Monto;
                }

                viewModel.TotalIngresos = i;
                viewModel.TotalEgresos = e;
                viewModel.Diferencia = i - e;
                ViewData["CurrentFilter"] = searchString;
            }
            if (!String.IsNullOrEmpty(searchDesde) && !String.IsNullOrEmpty(searchHasta))
            {
                DateTime fechaDesde = DateTime.Parse(searchDesde);
                DateTime fechaHasta = DateTime.Parse(searchHasta);
                caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);

                for (int it = caja.Movimientos.Count() - 1; it >= 0; it--)
                {
                    if (!(caja.Movimientos.ElementAt(it).Fecha.Date >= fechaDesde.Date && caja.Movimientos.ElementAt(it).Fecha.Date <= fechaHasta.Date))
                    {
                        caja.Movimientos.Remove(caja.Movimientos.ElementAt(it));
                    }
                }

                i = 0; e = 0; // i = ingreso, e = egreso
                foreach (var mov in caja.Movimientos)
                {
                    if (mov.Accion == "Ingreso")
                        i += mov.Monto;
                    else
                        e += mov.Monto;
                }

                viewModel.TotalIngresos = i;
                viewModel.TotalEgresos = e;
                viewModel.Diferencia = i - e;

                ViewData["CurrentFilterDesde"] = searchDesde;
                ViewData["CurrentFilterHasta"] = searchHasta;
            }

            if (viewModel.Caja == null)
            {
                return NotFound();
            }
            DateTime today = DateTime.Now;
            string customSwitches = string.Format(" --allow {0} --footer-html {0} --footer-spacing 0",
                Url.Action("FooterPDF", "Cajas", new { area = "" }, "http"));
            return new ViewAsPdf("ReporteCajaPDF", viewModel)
            {                
                FileName = today+"-"+ viewModel.Caja.Nombre + ".pdf",
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
        // GET: Cajas/Details/5
        public async Task<IActionResult> Details(int? id,            
            string searchString,
            string searchDesde,
            string searchHasta)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new CajaMovimientos();        
            var caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);           
            decimal i = 0, e = 0; // i = ingreso, e = egreso
            foreach (var mov in caja.Movimientos)
            {
                if (mov.Accion == "Ingreso")
                    i += mov.Monto;
                else
                    e += mov.Monto;
            }

            viewModel.Caja = caja;
            viewModel.Movimiento = new Movimiento();
            viewModel.TotalIngresos = i;
            viewModel.TotalEgresos = e;
            viewModel.TotalCaja = i - e;
            viewModel.Diferencia = i - e;
            if (!String.IsNullOrEmpty(searchString))
            {
                DateTime fecha = DateTime.Parse(searchString);
                caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);

                for (int it = caja.Movimientos.Count() - 1; it >= 0; it--)
                {
                    if(!(caja.Movimientos.ElementAt(it).Fecha.Date == fecha.Date))
                    {
                        caja.Movimientos.Remove(caja.Movimientos.ElementAt(it));
                    }
                }
                i = 0; e = 0; // i = ingreso, e = egreso
                foreach (var mov in caja.Movimientos)
                {
                    if (mov.Accion == "Ingreso")
                        i += mov.Monto;
                    else
                        e += mov.Monto;
                }
                
                viewModel.TotalIngresos = i;
                viewModel.TotalEgresos = e;
                viewModel.Diferencia = i - e;
                ViewData["CurrentFilter"] = searchString;
            }
            if (!String.IsNullOrEmpty(searchDesde) && !String.IsNullOrEmpty(searchHasta))
            {
                DateTime fechaDesde = DateTime.Parse(searchDesde);
                DateTime fechaHasta = DateTime.Parse(searchHasta);
                caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);

                for (int it = caja.Movimientos.Count() - 1; it >= 0; it--)
                {
                    if (!(caja.Movimientos.ElementAt(it).Fecha.Date >= fechaDesde.Date && caja.Movimientos.ElementAt(it).Fecha.Date <= fechaHasta.Date))
                    {
                        caja.Movimientos.Remove(caja.Movimientos.ElementAt(it));
                    }
                }

                i = 0; e = 0; // i = ingreso, e = egreso
                foreach (var mov in caja.Movimientos)
                {
                    if (mov.Accion == "Ingreso")
                        i += mov.Monto;
                    else
                        e += mov.Monto;
                }

                viewModel.TotalIngresos = i;
                viewModel.TotalEgresos = e;
                viewModel.Diferencia = i - e;

                ViewData["CurrentFilterDesde"] = searchDesde;
                ViewData["CurrentFilterHasta"] = searchHasta;
            }
            
            if (viewModel.Caja == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Cajas/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {            
            //SelectList selectList2 = new SelectList(_context.Set<Usuario>(), "ID", "NombreUsuario");
            SelectList selectList = new SelectList(_context.Set<Usuario>(), "ID", "NombreUsuario");
            
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).Single();
            if (usuario != null)
            {
                /*
                selectList = new SelectList(selectList2
                              .Where(x => x.Value.ToString() == usuario.ID.ToString()),
                              "ID",
                              "NombreUsuario");
                */
                var cajas = _context.Caja.Include(c => c.Usuario);               
                
                foreach (var item in selectList)
                {
                    foreach (var caja in cajas)
                    {
                        if (item.Value.ToString() == caja.UsuarioID.ToString())
                        {
                            item.Disabled = true;
                        }
                    }
                                       
                }
            }

            ViewData["UsuarioID"] = selectList;
            return View();
        }

        // POST: Cajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,MontoEfectivo,UsuarioID")] Caja caja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioID"] = new SelectList(_context.Set<Usuario>(), "ID", "NombreUsuario", caja.UsuarioID);
            return View(caja);
        }

        // GET: Cajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caja = await _context.Caja.FindAsync(id);
            if (caja == null)
            {
                return NotFound();
            }
            ViewData["UsuarioID"] = new SelectList(_context.Set<Usuario>(), "ID", "ApellidoPaterno", caja.UsuarioID);
            return View(caja);
        }

        // POST: Cajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,MontoEfectivo,UsuarioID")] Caja caja)
        {
            if (id != caja.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caja);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioID"] = new SelectList(_context.Set<Usuario>(), "ID", "ApellidoPaterno", caja.UsuarioID);
            return View(caja);
        }

        // GET: Cajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caja = await _context.Caja
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (caja == null)
            {
                return NotFound();
            }

            return View(caja);
        }

        // POST: Cajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caja = await _context.Caja.FindAsync(id);
            _context.Caja.Remove(caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CajaExists(int id)
        {
            return _context.Caja.Any(e => e.ID == id);
        }
    }
}
