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
    public class TutoresController : Controller
    {
        private readonly SchoolContext _context;

        public TutoresController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Tutores
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ApPatParm"] = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";
            ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "nom";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var tutores = from t in _context.Tutores.Include(t => t.Escolaridad)
                              .Include(t => t.Estudiantes) select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                tutores = tutores.Where(s => (
                    searchString.Contains(s.Nombre) & searchString.Contains(s.ApellidoPaterno) & searchString.Contains(s.ApellidoMaterno))
                    || (searchString.Contains(s.Nombre) & searchString.Contains(s.ApellidoPaterno)
                        || searchString.Contains(s.ApellidoPaterno) & searchString.Contains(s.ApellidoMaterno))
                    || s.Nombre.Contains(searchString)
                    || s.ApellidoPaterno.Contains(searchString)
                    || s.ApellidoMaterno.Contains(searchString)
                    || s.Matricula.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "app_desc":
                    tutores = tutores.OrderByDescending(s => s.ApellidoPaterno);
                    break;
                case "nom_desc":
                    tutores = tutores.OrderByDescending(s => s.Nombre);
                    break;
                case "nom":
                    ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom" : "nom_desc";
                    tutores = tutores.OrderBy(s => s.Nombre);
                    break;
                default:
                    tutores = tutores.OrderBy(s => s.ApellidoPaterno);
                    break;
            }
            int pageSize = 4;
            
            //var schoolContext = _context.Tutores.Include(t => t.Escolaridad);
            return View(await PaginatedList<Tutor>.CreateAsync(tutores.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Tutores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores
                .Include(t => t.Escolaridad)
                .Include(t => t.Estudiantes)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (tutor == null)
            {
                return NotFound();
            }

            DateTime nacimiento = tutor.FechaNacimiento;
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            if (tutor.Edad != edad)
            {
                tutor.Edad = edad;
                _context.Update(tutor);
                await _context.SaveChangesAsync();
            }

            return View(tutor);
        }

        // GET: Tutores/Create
        public IActionResult Create()
        {
            ViewData["EscolaridadID"] = new SelectList(_context.Escolaridades, "ID", "Nombre");
            return View();
        }

        // POST: Tutores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EscolaridadID,Direccion,Ocupacion,TelefonoCelular,TelefonoCasa,TelefonoTrabajo,CorreoElectronico,Facebook,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutor);
                await _context.SaveChangesAsync();
                //Asigna la atrícula automática
                tutor.Matricula = tutor.CrearMatricula("T", tutor.ID);
                try
                {
                    DateTime nacimiento = tutor.FechaNacimiento;
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    if (tutor.Edad != edad)
                    {
                        tutor.Edad = edad;
                        _context.Update(tutor);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(tutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(tutor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = tutor.ID });
            }
            ViewData["EscolaridadID"] = new SelectList(_context.Escolaridades, "ID", "Nombre", tutor.EscolaridadID);
            return View(tutor);
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            ViewData["EscolaridadID"] = new SelectList(_context.Escolaridades, "ID", "Nombre", tutor.EscolaridadID);
            return View(tutor);
        }

        // POST: Tutores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EscolaridadID,Direccion,Ocupacion,TelefonoCelular,TelefonoCasa,TelefonoTrabajo,CorreoElectronico,Facebook,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Tutor tutor)
        {
            if (id != tutor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime nacimiento = tutor.FechaNacimiento;
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    if (tutor.Edad != edad)
                    {
                        tutor.Edad = edad;
                        _context.Update(tutor);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(tutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(tutor.ID))
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
            ViewData["EscolaridadID"] = new SelectList(_context.Escolaridades, "ID", "Nombre", tutor.EscolaridadID);
            return View(tutor);
        }

        // GET: Tutores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores
                .Include(t => t.Escolaridad)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // POST: Tutores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            _context.Tutores.Remove(tutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorExists(int id)
        {
            return _context.Tutores.Any(e => e.ID == id);
        }
    }
}
