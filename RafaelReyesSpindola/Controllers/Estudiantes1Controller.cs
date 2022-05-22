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
    public class Estudiantes1Controller : Controller
    {
        private readonly SchoolContext _context;

        public Estudiantes1Controller(SchoolContext context)
        {
            _context = context;
        }

        // GET: Estudiantes1
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Estudiantes.Include(e => e.EscuelaProcedencia).Include(e => e.TipoSangre).Include(e => e.Tutor);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Estudiantes1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EscuelaProcedencia)
                .Include(e => e.TipoSangre)
                .Include(e => e.Tutor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes1/Create
        public IActionResult Create()
        {
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre");
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo");
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "ApellidoPaterno");
            return View();
        }

        // POST: Estudiantes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EscuelaProcedenciaID,PromedioProcedencia,TipoSangreID,TutorID,ParentescoTutor,Enfermedades,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre", estudiante.EscuelaProcedenciaID);
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo", estudiante.TipoSangreID);
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "ApellidoPaterno", estudiante.TutorID);
            return View(estudiante);
        }

        // GET: Estudiantes1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre", estudiante.EscuelaProcedenciaID);
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo", estudiante.TipoSangreID);
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "ApellidoPaterno", estudiante.TutorID);
            return View(estudiante);
        }

        // POST: Estudiantes1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EscuelaProcedenciaID,PromedioProcedencia,TipoSangreID,TutorID,ParentescoTutor,Enfermedades,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Estudiante estudiante)
        {
            if (id != estudiante.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.ID))
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
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre", estudiante.EscuelaProcedenciaID);
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo", estudiante.TipoSangreID);
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "ApellidoPaterno", estudiante.TutorID);
            return View(estudiante);
        }

        // GET: Estudiantes1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EscuelaProcedencia)
                .Include(e => e.TipoSangre)
                .Include(e => e.Tutor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.ID == id);
        }
    }
}
