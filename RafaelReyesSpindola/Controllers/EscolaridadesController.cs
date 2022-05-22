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
    [Authorize(Roles = "Administrador")]
    public class EscolaridadesController : Controller
    {
        private readonly SchoolContext _context;
        public EscolaridadesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Escolaridades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Escolaridades.ToListAsync());
        }

        // GET: Escolaridades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escolaridad = await _context.Escolaridades
                .FirstOrDefaultAsync(m => m.ID == id);
            if (escolaridad == null)
            {
                return NotFound();
            }

            return View(escolaridad);
        }

        // GET: Escolaridades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Escolaridades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre")] Escolaridad escolaridad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(escolaridad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(escolaridad);
        }

        // GET: Escolaridades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escolaridad = await _context.Escolaridades.FindAsync(id);
            if (escolaridad == null)
            {
                return NotFound();
            }
            return View(escolaridad);
        }

        // POST: Escolaridades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre")] Escolaridad escolaridad)
        {
            if (id != escolaridad.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escolaridad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscolaridadExists(escolaridad.ID))
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
            return View(escolaridad);
        }

        // GET: Escolaridades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escolaridad = await _context.Escolaridades
                .FirstOrDefaultAsync(m => m.ID == id);
            if (escolaridad == null)
            {
                return NotFound();
            }

            return View(escolaridad);
        }

        // POST: Escolaridades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var escolaridad = await _context.Escolaridades.FindAsync(id);
            _context.Escolaridades.Remove(escolaridad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscolaridadExists(int id)
        {
            return _context.Escolaridades.Any(e => e.ID == id);
        }
    }
}
