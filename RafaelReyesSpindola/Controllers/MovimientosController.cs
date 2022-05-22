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

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class MovimientosController : Controller
    {
        private readonly SchoolContext _context;

        public MovimientosController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Movimientos
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Movimiento.Include(m => m.Caja);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Movimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento
                .Include(m => m.Caja)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // GET: Movimientos/Create
        public IActionResult Create(int? id)
        {
            
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;            
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).Single();
            var selectList = new SelectList(_context.Caja, "ID", "Nombre");
            if (usuario != null && id!=null)
            {
                var cajas = _context.Caja.Where(
                        c => c.UsuarioID == usuario.ID); //only a box - 
                try
                {
                    var caja = cajas.Where(c => c.ID == id).Single();
                    foreach (var item in selectList)
                    {
                        if (item.Value.ToString() == caja.ID.ToString())
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Disabled = true;
                        }
                    }
                }
                catch (InvalidOperationException)
                {                    
                    var caja = _context.Caja.Where(c => c.ID == id).Single();
                    ModelState.AddModelError(string.Empty, "No tiene autorización para añadir el movimiento");
                    return RedirectToAction(nameof(Details), "Cajas", new { ID = caja.ID});
                }
                
            }
            ViewData["CajaID"] = selectList;
            return View();
        }

        // POST: Movimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CajaID,Accion,Monto,Fecha,TipoMovimiento")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                int CajaID = movimiento.CajaID;                

                //SUMAR O RESTAR EFECTIVO A LA CAJA
                var caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == CajaID);
                try
                {
                    bool exedido = false;
                    if (movimiento.Accion == "Ingreso")
                    {
                        caja.MontoEfectivo = caja.MontoEfectivo + movimiento.Monto;
                    }
                    else if (movimiento.Accion == "Egreso")
                    {                        
                        if(caja.MontoEfectivo >= movimiento.Monto)
                        {
                            caja.MontoEfectivo = caja.MontoEfectivo - movimiento.Monto;
                        }
                        else
                        {
                            exedido = true;
                            ModelState.AddModelError(string.Empty, "No hay suficiente efectivo para retirar esa cantidad");
                        }
                    }
                    if (!exedido)
                    {
                        _context.Update(caja);
                        await _context.SaveChangesAsync();
                        //Si se hizo la transaccion en la caja, se registra el movimiento
                        movimiento.ID = 0;
                        _context.Add(movimiento);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), "Cajas", new { ID = CajaID });
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
                
            }
            ViewData["CajaID"] = new SelectList(_context.Caja, "ID", "ID", movimiento.CajaID);
            return View(movimiento);
        }

        // GET: Movimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).Single();
            var selectList = new SelectList(_context.Caja, "ID", "Nombre");
            if (usuario != null)
            {
                var caja = _context.Caja.Where(
                        c => c.UsuarioID == usuario.ID).Single();
                foreach (var item in selectList)
                {
                    if (item.Value.ToString() == caja.ID.ToString())
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["CajaID"] = selectList;           
            return View(movimiento);
        }

        // POST: Movimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CajaID,Accion,Monto,Fecha,TipoMovimiento")] Movimiento movimiento)
        {
            if (id != movimiento.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {               
                // Se actualiza el movimiento 
                try
                {
                    _context.Update(movimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientoExists(movimiento.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                //Preparando datos del la Caja                                          
                var caja = await _context.Caja
                .Include(c => c.Movimientos)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == movimiento.CajaID);
                
                if(caja != null)
                {
                    // Calcular nuevo monto de la caja
                    decimal totalIngresos = 0;
                    decimal totalEgresos = 0;
                    var movCaja = _context.Movimiento
                    .Include(m => m.Caja).Where(m =>  m.CajaID == caja.ID);
                    foreach (var item in movCaja)
                    {
                        if(item.ID != movimiento.ID)
                        {
                            if (item.Accion == "Ingreso")
                                totalIngresos += item.Monto;
                            else
                                totalEgresos += item.Monto;
                        }
                    }                    
                    var totalNuevoCaja = totalIngresos - totalEgresos;
                    if (movimiento.Accion == "Ingreso")
                        totalNuevoCaja += movimiento.Monto;
                    else
                        totalNuevoCaja -= movimiento.Monto;
                    try
                    {
                        caja.MontoEfectivo = totalNuevoCaja;
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
                }
                
                return RedirectToAction(nameof(Details), "Cajas", new { ID = movimiento.CajaID });
                //return RedirectToAction(nameof(Index));

            }
            ViewData["CajaID"] = new SelectList(_context.Caja, "ID", "Nombre", movimiento.CajaID);
            return View(movimiento);
        }

        // GET: Movimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento
                .Include(m => m.Caja)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // POST: Movimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimiento = await _context.Movimiento.FindAsync(id);
            _context.Movimiento.Remove(movimiento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoExists(int id)
        {
            return _context.Movimiento.Any(e => e.ID == id);
        }
        private bool CajaExists(int id)
        {
            return _context.Caja.Any(e => e.ID == id);
        }
    }
}
