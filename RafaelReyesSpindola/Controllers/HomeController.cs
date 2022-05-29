using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Models;

namespace RafaelReyesSpindola.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Retrasos()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
            int pageSize = 8;

            //return View(await estudiantes.AsNoTracking().ToListAsync());
            return View(await PaginatedList<Estudiante>.CreateAsync(estudiantes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
