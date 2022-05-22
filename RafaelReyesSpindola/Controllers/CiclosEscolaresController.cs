using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CiclosEscolaresController : Controller
    {
        private readonly SchoolContext _context;

        public CiclosEscolaresController(SchoolContext context)
        {
            _context = context;
        }

        // GET: CiclosEscolares
        public async Task<IActionResult> Index()
        {
            return View(await _context.CiclosEscolares.ToListAsync());
        }

        // GET: CiclosEscolares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cicloEscolar = await _context.CiclosEscolares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cicloEscolar == null)
            {
                return NotFound();
            }

            return View(cicloEscolar);
        }

        // GET: CiclosEscolares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CiclosEscolares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FechaInicio,FechaFin")] CicloEscolar cicloEscolar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cicloEscolar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cicloEscolar);
        }

        // GET: CiclosEscolares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cicloEscolar = await _context.CiclosEscolares.FindAsync(id);
            if (cicloEscolar == null)
            {
                return NotFound();
            }
            return View(cicloEscolar);
        }

        // POST: CiclosEscolares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FechaInicio,FechaFin")] CicloEscolar cicloEscolar)
        {
            if (id != cicloEscolar.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cicloEscolar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CicloEscolarExists(cicloEscolar.ID))
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
            return View(cicloEscolar);
        }

        // GET: CiclosEscolares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cicloEscolar = await _context.CiclosEscolares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cicloEscolar == null)
            {
                return NotFound();
            }

            return View(cicloEscolar);
        }

        // POST: CiclosEscolares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cicloEscolar = await _context.CiclosEscolares.FindAsync(id);
            _context.CiclosEscolares.Remove(cicloEscolar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CicloEscolarExists(int id)
        {
            return _context.CiclosEscolares.Any(e => e.ID == id);
        }
    }
}
