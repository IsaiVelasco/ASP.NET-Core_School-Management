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
    [Authorize(Roles = "Administrador, Cajero")]
    public class CortesDeCajasController : Controller
    {
        private readonly SchoolContext _context;

        public CortesDeCajasController(SchoolContext context)
        {
            _context = context;
        }

        // GET: CortesDeCajas
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.CorteDeCaja.Include(c => c.Caja).Include(c => c.Usuario);
            return View(await schoolContext.ToListAsync());
        }

        // GET: CortesDeCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corteDeCaja = await _context.CorteDeCaja
                .Include(c => c.Caja)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (corteDeCaja == null)
            {
                return NotFound();
            }

            return View(corteDeCaja);
        }

        // GET: CortesDeCajas/Create
        public IActionResult Create(int?id)
        {
            var selectList = new SelectList(_context.Caja, "ID", "Nombre");
            CorteDeCaja corteDeCaja = new CorteDeCaja();
            
            if (id != null)
            {
                var caja = _context.Caja.Where(
                        c => c.ID == id)
                        .Include(c => c.Movimientos)
                        .Include(c => c.Usuario)
                        .Single();
                corteDeCaja.Caja = caja;
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
            var selectUser = new SelectList(_context.Usuario, "ID", "NombreUsuario");
            var userID = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _context.Usuario.Where(
                        u => u.ID == int.Parse(userID)).Single();
            if (usuario != null)
            {
                foreach (var item in selectUser)
                {
                    if (item.Value.ToString() == usuario.ID.ToString())
                    {
                        item.Selected = true;                        
                    }
                    else
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["UsuarioID"] = selectUser;

            if (corteDeCaja.Caja != null)
            {
                return View(corteDeCaja);
            }
            else
            {
                return Redirect("/Cajas");
            }
        }

        // POST: CortesDeCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CajaID,UsuarioID,Fecha,Contado,Calculado,Diferencia,B1000,B500,B200,B100,B50,B20,M10,M5,M2,M1,C50,C20,C10")] CorteDeCaja corteDeCaja)
        {
            int ? cajaID = corteDeCaja.ID;
            if (ModelState.IsValid)
            {
                corteDeCaja.ID = 0;
                _context.Add(corteDeCaja);
                await _context.SaveChangesAsync();
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = corteDeCaja.ID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CajaID"] = new SelectList(_context.Caja, "ID", "ID", corteDeCaja.CajaID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuario, "ID", "ApellidoPaterno", corteDeCaja.UsuarioID);
            
            if(cajaID != null)
            {
                var caja = _context.Caja.Where(
                        c => c.ID == cajaID)
                        .Include(c => c.Movimientos)
                        .Include(c => c.Usuario)
                        .Single();
                corteDeCaja.Caja = caja;
            }
            else
            {
                corteDeCaja.Caja = new Caja();
            }            
            return View(corteDeCaja);
        }

        // GET: CortesDeCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corteDeCaja = await _context.CorteDeCaja.FindAsync(id);
            if (corteDeCaja == null)
            {
                return NotFound();
            }
            ViewData["CajaID"] = new SelectList(_context.Caja, "ID", "ID", corteDeCaja.CajaID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuario, "ID", "ApellidoPaterno", corteDeCaja.UsuarioID);
            return View(corteDeCaja);
        }

        // POST: CortesDeCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CajaID,UsuarioID,Fecha,Contado,Calculado,Diferencia,B1000,B500,B200,B100,B50,B20,M10,M5,M2,M1,C50,C20,C10")] CorteDeCaja corteDeCaja)
        {
            if (id != corteDeCaja.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(corteDeCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorteDeCajaExists(corteDeCaja.ID))
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
            ViewData["CajaID"] = new SelectList(_context.Caja, "ID", "ID", corteDeCaja.CajaID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuario, "ID", "ApellidoPaterno", corteDeCaja.UsuarioID);
            return View(corteDeCaja);
        }

        // GET: CortesDeCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corteDeCaja = await _context.CorteDeCaja
                .Include(c => c.Caja)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (corteDeCaja == null)
            {
                return NotFound();
            }

            return View(corteDeCaja);
        }

        // POST: CortesDeCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var corteDeCaja = await _context.CorteDeCaja.FindAsync(id);
            _context.CorteDeCaja.Remove(corteDeCaja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorteDeCajaExists(int id)
        {
            return _context.CorteDeCaja.Any(e => e.ID == id);
        }
    }
}
