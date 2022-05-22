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
    public class MateriasController : Controller
    {
        private readonly SchoolContext _context;

        public MateriasController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Materias
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Materias.Include(m => m.Grado).Include(m => m.TipoMateria);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Grado)
                .Include(m => m.TipoMateria)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Materias/Create
        public IActionResult Create()
        {
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre");
            ViewData["TipoMateriaID"] = new SelectList(_context.Set<TipoMateria>(), "ID", "Nombre");
            return View();
        }

        // POST: Materias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,TipoMateriaID,GradoID")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", materia.GradoID);
            ViewData["TipoMateriaID"] = new SelectList(_context.Set<TipoMateria>(), "ID", "Nombre", materia.TipoMateriaID);
            return View(materia);
        }

        // GET: Materias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", materia.GradoID);
            ViewData["TipoMateriaID"] = new SelectList(_context.Set<TipoMateria>(), "ID", "Nombre", materia.TipoMateriaID);
            return View(materia);
        }

        // POST: Materias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,TipoMateriaID,GradoID")] Materia materia)
        {
            if (id != materia.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.ID))
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
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", materia.GradoID);
            ViewData["TipoMateriaID"] = new SelectList(_context.Set<TipoMateria>(), "ID", "Nombre", materia.TipoMateriaID);
            return View(materia);
        }

        // GET: Materias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Grado)
                .Include(m => m.TipoMateria)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaExists(int id)
        {
            return _context.Materias.Any(e => e.ID == id);
        }
    }
}
