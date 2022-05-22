using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using RafaelReyesSpindola.Models.SchoolViewModels;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Microsoft.AspNetCore.Authorization;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly SchoolContext _context;

        public InscripcionesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Inscripciones
        public async Task<IActionResult> Index(
            string SearchAtributo,
            int SearchCiclo,
            string SearchGrado)
        {
            ViewData["CurrentFilter"] = SearchAtributo;
            @ViewData["CurrentFilterCiclo"] = SearchCiclo;
            ViewData["CurrentFilterGrado"] = SearchGrado;
          
            var schoolContext = from s in _context.Inscripciones.Include(i => i.CicloEscolar)
                .Include(i => i.Estudiante)
                .Include(i => i.Grado)
                .Include(i => i.Grupo)
                                select s;
            if (!String.IsNullOrEmpty(SearchAtributo))
            {

                schoolContext = schoolContext.Where(s => (
                       SearchAtributo.Contains(s.Estudiante.Nombre)) || s.Estudiante.Nombre.Contains(SearchAtributo)
                    || SearchAtributo.Contains(s.Estudiante.Matricula) 
                    || SearchAtributo.Contains(s.Estudiante.Nombre+" "+s.Estudiante.ApellidoPaterno +" "+ s.Estudiante.ApellidoMaterno) ||(s.Estudiante.Nombre + " " + s.Estudiante.ApellidoPaterno + " " + s.Estudiante.ApellidoMaterno).Contains(SearchAtributo)
                    
                    || SearchAtributo.Contains(s.Folio) || s.Folio.Contains(SearchAtributo)
                    );
            }
            if (SearchCiclo != 0)
            {

                schoolContext = schoolContext.Where(s => (
                       SearchCiclo == s.CicloEscolar.ID
                    ));
            }
            if (!String.IsNullOrEmpty(SearchGrado))
            {

                schoolContext = schoolContext.Where(s => (
                       SearchGrado.Contains(s.Grado.Nombre)
                    ));
            }
            return View(await schoolContext.ToListAsync());
        }
        public async Task<ActionResult> FichaPDF(int? id)
        {
            //Carga de datos para la ficha de inscripcion
            var inscripcion = await _context.Inscripciones
                .Include(i => i.CicloEscolar)
                .Include(i => i.Estudiante)
                .ThenInclude(e => e.EscuelaProcedencia)
                .Include(i => i.Grado)
                .Include(i => i.Grupo)
                    .ThenInclude(g=> g.Grado)
                .FirstOrDefaultAsync(m => m.ID == id);
            var viewModel = new InscripcionData();
            viewModel.Inscripcion = inscripcion;
            var tutor = await _context.Tutores
                .Include(t => t.Escolaridad)
                .Include(t => t.Estudiantes)
                .FirstOrDefaultAsync(m => m.ID == inscripcion.Estudiante.TutorID);
            viewModel.Tutor = tutor;
            var estudiante = await _context.Estudiantes
                .Include(e => e.EscuelaProcedencia)
                .Include(e => e.TipoSangre)
                .Include(e => e.Tutor)
                .FirstOrDefaultAsync(m => m.ID == inscripcion.EstudianteID);
            viewModel.Estudiante = estudiante;
            var horario = await _context.Horario
                .Include(h => h.FilasHorarios)
                .Include(h => h.Grupo)
                .ThenInclude(h => h.Grado)
                .FirstOrDefaultAsync(m => m.ID == inscripcion.GradoID);
            return new ViewAsPdf("FichaPDF", viewModel)
            {
                FileName = viewModel.Estudiante.Matricula.ToString() + ".pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Margins(10, 16, 0, 16),
            };
        }
        // GET: Inscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Incluyendo los datos necesarios para crear el PDF
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

        // GET: Inscripciones/Create
        [Authorize(Roles = "Administrador, Director")]
        public IActionResult Create(int? id)
        {
            var selectList = new SelectList(_context.Estudiantes, "ID", "NombreMatricula");
            if (id != null)
            {
                var estudiantes = _context.Estudiantes
                .Include(e => e.EscuelaProcedencia)
                .Include(e => e.TipoSangre)
                .Include(e => e.Tutor);
                Estudiante estudiante = estudiantes.Where(
                        t => t.ID == id.Value).Single();
                foreach (var item in selectList)
                {
                    if (item.Value.ToString() == estudiante.ID.ToString())
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "InicioFin");
            ViewData["EstudianteID"] = selectList;
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre");
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "GradoYGrupo");
            return View();
        }

        // POST: Inscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Director")]
        public async Task<IActionResult> Create([Bind("ID,FechaInscripcion,EstudianteID,CicloEscolarID,GradoID,GrupoID,Folio")] Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                inscripcion.ID = 0;
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();

                inscripcion.Folio = inscripcion.CrearMatricula(inscripcion.ID);
                _context.Update(inscripcion);
                await _context.SaveChangesAsync();
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = inscripcion.ID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // GET: Inscripciones/Edit/5
        [Authorize(Roles = "Administrador, Director")]
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
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "InicioFin", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "NombreCompleto", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "GradoYGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // POST: Inscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Director")]
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
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = inscripcion.ID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CicloEscolarID"] = new SelectList(_context.CiclosEscolares, "ID", "ID", inscripcion.CicloEscolarID);
            ViewData["EstudianteID"] = new SelectList(_context.Estudiantes, "ID", "ApellidoPaterno", inscripcion.EstudianteID);
            ViewData["GradoID"] = new SelectList(_context.Grados, "ID", "Nombre", inscripcion.GradoID);
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", inscripcion.GrupoID);
            return View(inscripcion);
        }

        // GET: Inscripciones/Delete/5
        [Authorize(Roles = "Administrador")]
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

        // POST: Inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
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
