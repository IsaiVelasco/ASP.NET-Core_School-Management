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
using System.Diagnostics;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class HorariosController : Controller
    {
        private readonly SchoolContext _context;

        public HorariosController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HorarioPDF(int? id)
        {
            var viewModel = new ScheduleWithLines();
            var horario = await _context.Horario
                .Include(h => h.FilasHorarios)
                .Include(h => h.Grupo)
                .ThenInclude(h => h.Grado)
                .FirstOrDefaultAsync(m => m.ID == id);

            viewModel.Horario = horario;
            // return View(await _context.Customers.ToListAsync());
            return new ViewAsPdf("HorarioPDF", viewModel)
            {
                FileName = viewModel.Horario.GradoYGrupo + ".pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }
        // GET: Horarios
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Horario.Include(h => h.Grupo).ThenInclude(h => h.Grado);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new ScheduleWithLines();
            var horario = await _context.Horario
                .Include(h => h.FilasHorarios)
                .Include(h => h.Grupo)
                .ThenInclude(h => h.Grado)
                .FirstOrDefaultAsync(m => m.ID == id);

            viewModel.Horario = horario;
            viewModel.Fila = new FilaHorario();
            /*var horario = await _context.Horario
                .Include(h => h.FilasHorarios)
                .Include(h => h.Grupo)
                .ThenInclude(h => h.Grado)
                .FirstOrDefaultAsync(m => m.ID == id);*/
            if (viewModel.Horario == null)
            {
                return NotFound();
            }
            var materias = _context.Materias.Where(x=> x.GradoID == horario.Grupo.GradoID | x.Grado.Nombre == "General");            
            var selectList = new SelectList(materias, "Nombre", "Nombre");           
            ViewData["MateriaNombre"] = selectList;
            return View(viewModel);
        }
        // GET: FilasHorarios/Create
        public IActionResult CreateLine()
        {
            ViewData["HorarioID"] = new SelectList(_context.Horario, "ID", "ID");
            ViewData["DiaUno"] = new SelectList(_context.Materias, "ID", "Nombre");
            var selectList = new SelectList(_context.Materias, "Nombre", "Nombre");
            ViewData["MateriaNombre"] = selectList;
            return View();
        }

        // POST: FilasHorarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLine([Bind("ID,HorarioID,HoraEntrada,HoraSalida,DiaUno,DiaDos,DiaTres,DiaCuatro,DiaCinco,DiaSeis,DiaSiete")] FilaHorario filaHorario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filaHorario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HorarioID"] = new SelectList(_context.Horario, "ID", "ID", filaHorario.HorarioID);
            return View(filaHorario);
        }
        // GET: Horarios/Create
        public IActionResult Create()
        {
            var grupos = _context.Grupos;
            var grados = _context.Grados;
            foreach (var grupo in grupos)
            {
                grupo.Grado = grados.Where(
                        t => t.ID == grupo.GradoID).Single();
            }
            SelectList selectList = new SelectList(_context.Grupos, "ID", "GradoYGrupo");
            var horarios = _context.Horario;
            // ID, Grado, Grupo
            foreach (var item in selectList)
            {
                // Si hay un horario con ese grupo->deshabilitalo
                foreach (var horario in horarios)
                {
                    if(horario.GrupoID.ToString() == item.Value.ToString())
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["GrupoID"] = selectList;
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GrupoID")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { ID = horario.ID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", horario.GrupoID);
            //consultar el campo recien creado
            
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grupos = _context.Grupos;
            var grados = _context.Grados;
            foreach (var grupo in grupos)
            {
                grupo.Grado = grados.Where(
                        t => t.ID == grupo.GradoID).Single();
            }
            
            var horario = await _context.Horario.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "GradoYGrupo", horario.GrupoID);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GrupoID")] Horario horario)
        {
            if (id != horario.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.ID))
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
            ViewData["GrupoID"] = new SelectList(_context.Grupos, "ID", "NombreGrupo", horario.GrupoID);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horario
                .Include(h => h.Grupo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horario.FindAsync(id);
            _context.Horario.Remove(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
            return _context.Horario.Any(e => e.ID == id);
        }
        
    }    
}
