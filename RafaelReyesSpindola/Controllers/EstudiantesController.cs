using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class EstudiantesController : Controller
    {
        private readonly SchoolContext _context;

        public EstudiantesController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ContactPDF()
        {
            // return View(await _context.Customers.ToListAsync());
            return new ViewAsPdf("Index", await _context.Estudiantes.ToListAsync())
            {
                // ...
            };
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ApPatParm"] = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";
            ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "nom";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var estudiantes = from s in _context.Estudiantes.Include(e => e.EscuelaProcedencia)
                              .Include(e => e.TipoSangre).Include(e => e.Tutor)
                              .Include(e => e.Pagos)
                                .ThenInclude(p => p.ListaConceptos)
                                 .ThenInclude(cp => cp.ConceptoPago)
                             .Include(e => e.Inscripciones)
                                .ThenInclude(c => c.CicloEscolar)
                              select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                estudiantes = estudiantes.Where(s => (
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
                    estudiantes = estudiantes.OrderByDescending(s => s.ApellidoPaterno);
                    break;
                case "nom_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.Nombre);
                    break;
                case "nom":
                    ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom" : "nom_desc";
                    estudiantes = estudiantes.OrderBy(s => s.Nombre);
                    break;
                default:
                    estudiantes = estudiantes.OrderBy(s => s.ApellidoPaterno);
                    break;
            }

            foreach (var estudiante in estudiantes)
            {
                DateTime nacimiento = estudiante.FechaNacimiento;
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                if (estudiante.Edad != edad)
                {
                    estudiante.Edad = edad;
                }
            }
            int pageSize = 4;

            //return View(await estudiantes.AsNoTracking().ToListAsync());
            return View(await PaginatedList<Estudiante>.CreateAsync(estudiantes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Estudiantes/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EscuelaProcedencia)
                .Include(e => e.TipoSangre)
                .Include(e => e.Tutor) //
                .Include(e => e.Pagos)
                    .ThenInclude(p => p.ListaConceptos)
                        .ThenInclude(cp => cp.ConceptoPago)
                .Include(e => e.Inscripciones)
                    .ThenInclude(c => c.CicloEscolar)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (estudiante == null)
            {
                return NotFound();
            }
            DateTime nacimiento = estudiante.FechaNacimiento;
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            if (estudiante.Edad != edad)
            {
                estudiante.Edad = edad;
                _context.Update(estudiante);
                await _context.SaveChangesAsync();
            }
            /*
            // Render any HTML fragment or document to HTML
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
            var OutputPath = "HtmlToPDF.pdf";
            PDF.SaveAs(OutputPath);*/
            // This neat trick opens our PDF file so we can see the result in our default PDF viewer
            //System.Diagnostics.Process.Start(OutputPath);

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create(int? id)
        {
            var selectList = new SelectList(_context.Tutores, "ID", "NombreMatricula");
            if ( id != null)
            {
                var tutores = _context.Tutores
                .Include(t => t.Escolaridad)
                .Include(t => t.Estudiantes);
                Tutor tutor = tutores.Where(
                        t => t.ID == id.Value).Single();                
                foreach (var item in selectList)
                {
                    if (item.Value.ToString() == tutor.ID.ToString())
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre");
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo");
            ViewData["TutorID"] = selectList;
            return View();
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EscuelaProcedenciaID,PromedioProcedencia,TipoSangreID,TutorID,ParentescoTutor,Enfermedades,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                estudiante.ID = 0;
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync();
                //Asigna la atrícula automática
                estudiante.Matricula = estudiante.CrearMatricula("E", estudiante.ID);
                try
                {
                    DateTime nacimiento = estudiante.FechaNacimiento;
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    if (estudiante.Edad != edad)
                    {
                        estudiante.Edad = edad;
                        _context.Update(estudiante);
                        await _context.SaveChangesAsync();
                    }
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
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = estudiante.ID });
            }
            ViewData["EscuelaProcedenciaID"] = new SelectList(_context.EscuelasProcedencia, "ID", "Nombre", estudiante.EscuelaProcedenciaID);
            ViewData["TipoSangreID"] = new SelectList(_context.TiposSangre, "ID", "NombreTipo", estudiante.TipoSangreID);
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "NombreMatricula", estudiante.TutorID);
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
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
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "NombreMatricula", estudiante.TutorID);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
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
                    DateTime nacimiento = estudiante.FechaNacimiento;
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    if (estudiante.Edad != edad)
                    {
                        estudiante.Edad = edad;                        
                    }
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
            ViewData["TutorID"] = new SelectList(_context.Tutores, "ID", "NombreMatricula", estudiante.TutorID);
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
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

        // POST: Estudiantes/Delete/5
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

        // GET: Estudiantes
        public async Task<IActionResult> Retrasos(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ApPatParm"] = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";
            ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "nom";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var estudiantes = from s in _context.Estudiantes.Include(e => e.EscuelaProcedencia)
                              .Include(e => e.TipoSangre).Include(e => e.Tutor)
                              .Include(e => e.Pagos)
                                .ThenInclude(p => p.ListaConceptos)
                                 .ThenInclude(cp => cp.ConceptoPago)
                             .Include(e => e.Inscripciones)
                                .ThenInclude(c => c.CicloEscolar)
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                estudiantes = estudiantes.Where(s => (
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
                    estudiantes = estudiantes.OrderByDescending(s => s.ApellidoPaterno);
                    break;
                case "nom_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.Nombre);
                    break;
                case "nom":
                    ViewData["NomParm"] = String.IsNullOrEmpty(sortOrder) ? "nom" : "nom_desc";
                    estudiantes = estudiantes.OrderBy(s => s.Nombre);
                    break;
                default:
                    estudiantes = estudiantes.OrderBy(s => s.ApellidoPaterno);
                    break;
            }
            int pageSize = 4;

            //return View(await estudiantes.AsNoTracking().ToListAsync());
            return View(await PaginatedList<Estudiante>.CreateAsync(estudiantes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
