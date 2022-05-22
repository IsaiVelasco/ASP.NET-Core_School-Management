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
    [Authorize(Roles = "Administrador, Cajero")]
    public class EscuelasProcedenciaController : Controller
    {
        private readonly SchoolContext _context;

        public EscuelasProcedenciaController(SchoolContext context)
        {
            _context = context;
        }

        // GET: EscuelasProcedencia
        public async Task<IActionResult> Index()
        {
            return View(await _context.EscuelasProcedencia.ToListAsync());
        }

        // GET: EscuelasProcedencia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escuelaProcedencia = await _context.EscuelasProcedencia
                .FirstOrDefaultAsync(m => m.ID == id);
            if (escuelaProcedencia == null)
            {
                return NotFound();
            }

            return View(escuelaProcedencia);
        }

        // GET: EscuelasProcedencia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EscuelasProcedencia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre")] EscuelaProcedencia escuelaProcedencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(escuelaProcedencia);
                await _context.SaveChangesAsync();
                //consultar el campo recien creado
                return RedirectToAction(nameof(Details), new { ID = escuelaProcedencia.ID });
                //return RedirectToAction(nameof(Index));
            }
            return View(escuelaProcedencia);
        }

        // GET: EscuelasProcedencia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escuelaProcedencia = await _context.EscuelasProcedencia.FindAsync(id);
            if (escuelaProcedencia == null)
            {
                return NotFound();
            }
            return View(escuelaProcedencia);
        }

        // POST: EscuelasProcedencia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre")] EscuelaProcedencia escuelaProcedencia)
        {
            if (id != escuelaProcedencia.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escuelaProcedencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscuelaProcedenciaExists(escuelaProcedencia.ID))
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
            return View(escuelaProcedencia);
        }

        // GET: EscuelasProcedencia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escuelaProcedencia = await _context.EscuelasProcedencia
                .FirstOrDefaultAsync(m => m.ID == id);
            if (escuelaProcedencia == null)
            {
                return NotFound();
            }

            return View(escuelaProcedencia);
        }

        // POST: EscuelasProcedencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var escuelaProcedencia = await _context.EscuelasProcedencia.FindAsync(id);
            _context.EscuelasProcedencia.Remove(escuelaProcedencia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscuelaProcedenciaExists(int id)
        {
            return _context.EscuelasProcedencia.Any(e => e.ID == id);
        }
    }
}
