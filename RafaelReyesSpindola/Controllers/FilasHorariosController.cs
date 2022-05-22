using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class FilasHorariosController : Controller
    {
        private readonly SchoolContext _context;

        public FilasHorariosController(SchoolContext context)
        {
            _context = context;
        }

        // GET: FilasHorarios
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.FilaHorarios.Include(f => f.Horario);
            return View(await schoolContext.ToListAsync());
        }

        // GET: FilasHorarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filaHorario = await _context.FilaHorarios
                .Include(f => f.Horario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (filaHorario == null)
            {
                return NotFound();
            }

            return View(filaHorario);
        }

        // GET: FilasHorarios/Create
        public IActionResult Create(int? id)
        {
            var horarios = _context.Horario.Include(h => h.FilasHorarios);
            var grupos = _context.Grupos;
            var grados = _context.Grados;
            //Adigna a cada grupo el valor al objeto grado
            foreach (var grupo in grupos)
            {
                grupo.Grado = grados.Where(
                        t => t.ID == grupo.GradoID).Single();
            }
            //Asigna a cada horario, su objeto grupo correspondiente
            foreach (var horario in horarios)
            {
                horario.Grupo = grupos.Where(g => g.ID == horario.GrupoID).Single();                
            }
            var selectHorario = new SelectList(horarios, "ID", "GradoYGrupo");
            var materias = _context.Materias.Where(m => m.Grado.ID == 1);
            if (id!= null)
            {                
                
                Horario horario = horarios.Where(
                        h => h.ID == id.Value).Single();
                materias = _context.Materias.Where(m => m.Grado.ID == horario.Grupo.Grado.ID);
                foreach (var item in selectHorario)
                {
                    if (item.Value.ToString() == horario.ID.ToString())
                    {
                        item.Selected = true;                        
                    }
                    else
                    {
                        item.Disabled = true;
                    }
                }
            }
            ViewData["HorarioID"] = selectHorario;
            
            var selectList = new SelectList(materias, "Nombre", "Nombre").ToList();

            SelectListItem li = new SelectListItem("Homenaje", "Homenaje");
            SelectListItem li2 = new SelectListItem("Receso", "Receso");
            selectList.Add(li);
            selectList.Add(li2);
            ViewData["MateriaNombre"] = selectList;
            return View();
        }

        // POST: FilasHorarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,HorarioID,HoraEntrada,HoraSalida,DiaUno,DiaDos,DiaTres,DiaCuatro,DiaCinco,DiaSeis,DiaSiete")] FilaHorario filaHorario)
        {
            if (ModelState.IsValid)
            {
                int HorarioID = filaHorario.HorarioID;
                filaHorario.ID = 0; //Para quitar el id del Horario que llega desde la vista
                _context.Add(filaHorario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Horarios",new { ID = HorarioID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["HorarioID"] = new SelectList(_context.Horario, "ID", "ID", filaHorario.HorarioID);
            return View(filaHorario);
        }

        // GET: FilasHorarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filaHorario = await _context.FilaHorarios.FindAsync(id);
            if (filaHorario == null)
            {
                return NotFound();
            }
            ViewData["HorarioID"] = new SelectList(_context.Horario, "ID", "GradoYGrupo", filaHorario.HorarioID);            
            var selectList = new SelectList(_context.Materias, "Nombre", "Nombre").ToList();
            SelectListItem li = new SelectListItem("Homenaje", "Homenaje");
            SelectListItem li2 = new SelectListItem("Receso", "Receso");
            selectList.Add(li);
            selectList.Add(li2);
            ViewData["MateriaNombre"] = selectList;
            return View(filaHorario);
        }

        // POST: FilasHorarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,HorarioID,HoraEntrada,HoraSalida,DiaUno,DiaDos,DiaTres,DiaCuatro,DiaCinco,DiaSeis,DiaSiete")] FilaHorario filaHorario)
        {
            if (id != filaHorario.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filaHorario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilaHorarioExists(filaHorario.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), "Horarios", new { ID = filaHorario.HorarioID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["HorarioID"] = new SelectList(_context.Horario, "ID", "ID", filaHorario.HorarioID);
            return View(filaHorario);
        }

        // GET: FilasHorarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filaHorario = await _context.FilaHorarios
                .Include(f => f.Horario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (filaHorario == null)
            {
                return NotFound();
            }

            return View(filaHorario);
        }

        // POST: FilasHorarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            var filaHorario = await _context.FilaHorarios.FindAsync(id);            
            _context.FilaHorarios.Remove(filaHorario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "Horarios", new { ID = filaHorario.HorarioID });
            //return RedirectToAction(nameof(Index));
        }

        private bool FilaHorarioExists(int id)
        {
            return _context.FilaHorarios.Any(e => e.ID == id);
        }
    }
}
