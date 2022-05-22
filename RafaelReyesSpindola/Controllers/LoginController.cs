using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Helper;
using RafaelReyesSpindola.Models.SchoolViewModels;
namespace RafaelReyesSpindola.Controllers
{
    public class LoginController : Controller
    {
        private readonly SchoolContext _context;

        public LoginController(SchoolContext context)
        {
            _context = context;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home");
            }
            return View();
        }

     
        public async Task<IActionResult> Login([Bind("NombreUsuario,Password")] UsuarioVM usuarioVM)
        {
            if (ModelState.IsValid)
            {                
                var result = await _context.Usuario
                    .Include(x => x.RolesUsuario).ThenInclude(x => x.Rol)
                    .Where(x => x.NombreUsuario == usuarioVM.NombreUsuario)
                    .SingleOrDefaultAsync();
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Verifique su nombre de usuario");
                    
                }
                else
                {
                   if(HashHelper.CheckHash(usuarioVM.Password,result.Password, result.Sal))
                    {
                        if(result.RolesUsuario.Count > 0)
                        {
                            //return Ok();
                            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.ID.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.Name, result.NombreUsuario));
                            identity.AddClaim(new Claim(ClaimTypes.Email, result.Correo));
                            identity.AddClaim(new Claim("Dato", "Valor"));

                            foreach (var rol in result.RolesUsuario)
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, rol.Rol.Descripcion));
                                System.Diagnostics.Debug.WriteLine("ROL ASIGNADO: "+rol.Rol.Descripcion);
                            }

                            var principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                                new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddHours(1), IsPersistent = true });

                            return RedirectToAction(nameof(Index), "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Sin acceso al sistema. Consulte su administrador");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Contraseña incorrecta");
                    }
                }
            }
            return View("Index",usuarioVM);
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
