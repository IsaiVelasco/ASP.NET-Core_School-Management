using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;

namespace RafaelReyesSpindola.Controllers
{
    public class AlergiasController : Controller
    {
        private readonly SchoolContext _context;

        public AlergiasController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Alergias
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Alergia.Include(a => a.ClasificacionAlergia);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Alergias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alergia = await _context.Alergia
                .Include(a => a.ClasificacionAlergia)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (alergia == null)
            {
                return NotFound();
            }

            return View(alergia);
        }

        // GET: Alergias/Create
        public IActionResult Create()
        {
            ViewData["ClasificacionAlergiaID"] = new SelectList(_context.ClasificacionesAlergias, "ID", "NombreClasificacionAlergia");
            return View();
        }

        // POST: Alergias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClasificacionAlergiaID,Descripcion")] Alergia alergia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alergia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClasificacionAlergiaID"] = new SelectList(_context.ClasificacionesAlergias, "ID", "NombreClasificacionAlergia", alergia.ClasificacionAlergiaID);
            return View(alergia);
        }

        // GET: Alergias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alergia = await _context.Alergia.FindAsync(id);
            if (alergia == null)
            {
                return NotFound();
            }
            ViewData["ClasificacionAlergiaID"] = new SelectList(_context.ClasificacionesAlergias, "ID", "NombreClasificacionAlergia", alergia.ClasificacionAlergiaID);
            return View(alergia);
        }

        // POST: Alergias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClasificacionAlergiaID,Descripcion")] Alergia alergia)
        {
            if (id != alergia.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alergia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlergiaExists(alergia.ID))
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
            ViewData["ClasificacionAlergiaID"] = new SelectList(_context.ClasificacionesAlergias, "ID", "NombreClasificacionAlergia", alergia.ClasificacionAlergiaID);
            return View(alergia);
        }

        // GET: Alergias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alergia = await _context.Alergia
                .Include(a => a.ClasificacionAlergia)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (alergia == null)
            {
                return NotFound();
            }

            return View(alergia);
        }

        // POST: Alergias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alergia = await _context.Alergia.FindAsync(id);
            _context.Alergia.Remove(alergia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlergiaExists(int id)
        {
            return _context.Alergia.Any(e => e.ID == id);
        }
    }
}
