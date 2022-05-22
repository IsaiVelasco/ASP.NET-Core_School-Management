using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using Microsoft.AspNetCore.Authorization;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class TiposMateriasController : Controller
    {
        private readonly SchoolContext _context;

        public TiposMateriasController(SchoolContext context)
        {
            _context = context;
        }

        // GET: TiposMaterias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoMateria.ToListAsync());
        }

        // GET: TiposMaterias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMateria = await _context.TipoMateria
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipoMateria == null)
            {
                return NotFound();
            }

            return View(tipoMateria);
        }

        // GET: TiposMaterias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposMaterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre")] TipoMateria tipoMateria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoMateria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMateria);
        }

        // GET: TiposMaterias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMateria = await _context.TipoMateria.FindAsync(id);
            if (tipoMateria == null)
            {
                return NotFound();
            }
            return View(tipoMateria);
        }

        // POST: TiposMaterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre")] TipoMateria tipoMateria)
        {
            if (id != tipoMateria.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoMateria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoMateriaExists(tipoMateria.ID))
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
            return View(tipoMateria);
        }

        // GET: TiposMaterias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMateria = await _context.TipoMateria
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipoMateria == null)
            {
                return NotFound();
            }

            return View(tipoMateria);
        }

        // POST: TiposMaterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoMateria = await _context.TipoMateria.FindAsync(id);
            _context.TipoMateria.Remove(tipoMateria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoMateriaExists(int id)
        {
            return _context.TipoMateria.Any(e => e.ID == id);
        }
    }
}
