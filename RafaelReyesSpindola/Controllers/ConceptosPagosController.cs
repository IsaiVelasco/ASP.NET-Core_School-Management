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
    public class ConceptosPagosController : Controller
    {
        private readonly SchoolContext _context;

        public ConceptosPagosController(SchoolContext context)
        {
            _context = context;
        }

        // GET: ConceptosPagos
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConceptosPago.ToListAsync());
        }

        // GET: ConceptosPagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoPago = await _context.ConceptosPago
                .FirstOrDefaultAsync(m => m.ID == id);
            if (conceptoPago == null)
            {
                return NotFound();
            }

            return View(conceptoPago);
        }

        // GET: ConceptosPagos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConceptosPagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Tarifa,PeriodoPago")] ConceptoPago conceptoPago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conceptoPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conceptoPago);
        }

        // GET: ConceptosPagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoPago = await _context.ConceptosPago.FindAsync(id);
            if (conceptoPago == null)
            {
                return NotFound();
            }
            return View(conceptoPago);
        }

        // POST: ConceptosPagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Tarifa,PeriodoPago")] ConceptoPago conceptoPago)
        {
            if (id != conceptoPago.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conceptoPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConceptoPagoExists(conceptoPago.ID))
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
            return View(conceptoPago);
        }

        // GET: ConceptosPagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptoPago = await _context.ConceptosPago
                .FirstOrDefaultAsync(m => m.ID == id);
            if (conceptoPago == null)
            {
                return NotFound();
            }

            return View(conceptoPago);
        }

        // POST: ConceptosPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conceptoPago = await _context.ConceptosPago.FindAsync(id);
            _context.ConceptosPago.Remove(conceptoPago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConceptoPagoExists(int id)
        {
            return _context.ConceptosPago.Any(e => e.ID == id);
        }
    }
}
