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
    public class GradosController : Controller
    {
        private readonly SchoolContext _context;

        public GradosController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Grados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Grados.ToListAsync());
        }

        // GET: Grados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grado = await _context.Grados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (grado == null)
            {
                return NotFound();
            }

            return View(grado);
        }

        // GET: Grados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre")] Grado grado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grado);
        }

        // GET: Grados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grado = await _context.Grados.FindAsync(id);
            if (grado == null)
            {
                return NotFound();
            }
            return View(grado);
        }

        // POST: Grados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre")] Grado grado)
        {
            if (id != grado.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradoExists(grado.ID))
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
            return View(grado);
        }

        // GET: Grados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grado = await _context.Grados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (grado == null)
            {
                return NotFound();
            }

            return View(grado);
        }

        // POST: Grados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grado = await _context.Grados.FindAsync(id);
            _context.Grados.Remove(grado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradoExists(int id)
        {
            return _context.Grados.Any(e => e.ID == id);
        }
    }
}
