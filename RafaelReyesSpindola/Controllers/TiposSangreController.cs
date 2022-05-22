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
    using Microsoft.AspNetCore.Authorization;
    public class TiposSangreController : Controller
    {
        private readonly SchoolContext _context;

        public TiposSangreController(SchoolContext context)
        {
            _context = context;
        }

        // GET: TiposSangre
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposSangre.ToListAsync());
        }

        // GET: TiposSangre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TiposSangre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipoSangre == null)
            {
                return NotFound();
            }

            return View(tipoSangre);
        }

        // GET: TiposSangre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposSangre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,NombreTipo")] TipoSangre tipoSangre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoSangre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSangre);
        }

        // GET: TiposSangre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TiposSangre.FindAsync(id);
            if (tipoSangre == null)
            {
                return NotFound();
            }
            return View(tipoSangre);
        }

        // POST: TiposSangre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NombreTipo")] TipoSangre tipoSangre)
        {
            if (id != tipoSangre.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoSangre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSangreExists(tipoSangre.ID))
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
            return View(tipoSangre);
        }

        // GET: TiposSangre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TiposSangre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipoSangre == null)
            {
                return NotFound();
            }

            return View(tipoSangre);
        }

        // POST: TiposSangre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSangre = await _context.TiposSangre.FindAsync(id);
            _context.TiposSangre.Remove(tipoSangre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSangreExists(int id)
        {
            return _context.TiposSangre.Any(e => e.ID == id);
        }
    }
}
