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
    public class InscripcionsController : Controller
    {
        private readonly SchoolContext _context;

        public InscripcionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Inscripcions
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Inscripciones.Include(i => i.CicloEscolar).Include(i => i.Estudiante).Include(i => i.Grado).Include(i => i.Grupo);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Inscripcions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones
                .Include(i => i.CicloEscolar)
                .Include(i => i.Estudiante)
                .Include(i => i.Grado)
                .Include(i => i.Grupo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // GET: Inscripcions/Create
        public IActionResult Create()
        {
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID");
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno");
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre");
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo");
            return View();
        }

        // POST: Inscripcions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FechaInscripcion,EstudianteID,CicloEscolarID,GradoID,GrupoID,Folio")] Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // GET: Inscripcions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // POST: Inscripcions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FechaInscripcion,EstudianteID,CicloEscolarID,GradoID,GrupoID,Folio")] Inscripcion inscripcion)
        {
            if (id != inscripcion.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcionExists(inscripcion.ID))
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
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // GET: Inscripcions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones
                .Include(i => i.CicloEscolar)
                .Include(i => i.Estudiante)
                .Include(i => i.Grado)
                .Include(i => i.Grupo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // POST: Inscripcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripciones.Any(e => e.ID == id);
        }
    }
}
