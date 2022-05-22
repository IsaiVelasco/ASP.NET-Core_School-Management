using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RafaelReyesSpindola.Data;
using RafaelReyesSpindola.Helper;
using RafaelReyesSpindola.Models;
using RafaelReyesSpindola.Models.SchoolViewModels;

namespace RafaelReyesSpindola.Controllers
{
    
    public class UsuariosController : Controller
    {
        private readonly SchoolContext _context;
        
        public UsuariosController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Usuarios  
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        [Authorize(Roles = "Administrador, Director, Cajero")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.RolesUsuario).ThenInclude(r => r.Rol)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (usuario == null)
            {
                return NotFound();
            }
            DateTime nacimiento = usuario.FechaNacimiento;
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            if (usuario.Edad != edad)
            {
                usuario.Edad = edad;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            var usuario = new Usuario();
            usuario.RolesUsuario = new List<RolesDeUsuarios>();
            PopulateAssignedRoleData(usuario);
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario,Correo,Password,ID,Nombre,ApellidoPaterno,ApellidoMaterno,Edad,FechaNacimiento,Matricula")] Usuario usuario, string[] selectedRoles)
        {
            if (selectedRoles != null)
            {
                usuario.RolesUsuario = new List<RolesDeUsuarios>();
                foreach (var rol in selectedRoles)
                {
                    var RolToAdd = new RolesDeUsuarios { UsuarioID = usuario.ID, RolID = int.Parse(rol) };
                    usuario.RolesUsuario.Add(RolToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                var hash = HashHelper.Hash(usuario.Password);
                usuario.Password = hash.Password;
                usuario.Sal = hash.Salt;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                //Asigna la atrícula automática
                usuario.Matricula = usuario.CrearMatricula("U", usuario.ID);
                try
                {
                    DateTime nacimiento = usuario.FechaNacimiento;
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    if (usuario.Edad != edad)
                    {
                        usuario.Edad = edad;
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.ID))
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
            PopulateAssignedRoleData(usuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "Administrador, Director, Cajero")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.RolesUsuario).ThenInclude(r => r.Rol)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (usuario == null)
            {
                return NotFound();
            }
            usuario.Password = null;
            PopulateAssignedRoleData(usuario);
            return View(usuario);
        }

        private void PopulateAssignedRoleData(Usuario usuario)
        {
            var allRolesUsuario = _context.Roles;
            var usuarioRolesUsuario = new HashSet<int>(usuario.RolesUsuario.Select(c => c.RolID));
            var viewModel = new List<AssignedRoleData>();
            foreach (var rol in allRolesUsuario)
            {
                viewModel.Add(new AssignedRoleData
                {
                    RolID = rol.ID,
                    Descripcion = rol.Descripcion,
                    Assigned = usuarioRolesUsuario.Contains(rol.ID),
                });
            }
            ViewData["Roles"] = viewModel;
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador, Director, Cajero")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedRoles)
        {
            if (id == null)
            {
                return NotFound();
            }
            var usuarioToUpdate = await _context.Usuario
                .Include(p => p.RolesUsuario)
                    .ThenInclude(p => p.Rol)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Usuario>(
                usuarioToUpdate,
                "",
                p => p.NombreUsuario, p => p.Correo, p => p.Password, 
                p => p.Nombre, p => p.ApellidoPaterno, p => p.ApellidoPaterno,
                p => p.FechaNacimiento, p => p.Edad))
            {
                
                var result = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                if (HashHelper.CheckHash(usuarioToUpdate.Password, result.Password, result.Sal))
                {
                    
                    try
                    {
                        var hash = HashHelper.Hash(usuarioToUpdate.Password);
                        usuarioToUpdate.Password = hash.Password;
                        usuarioToUpdate.Sal = hash.Salt;
                        UpdateRolesUsuarios(selectedRoles, usuarioToUpdate);
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException /* ex */)
                        {
                            //Log the error (uncomment ex variable name and write a log.)
                            ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UsuarioExists(usuarioToUpdate.ID))
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Contraseña incorrecta. Imposible actualizar los cambios.");
                }
            }
            //UpdateRolesUsuarios(selectedRoles, usuarioToUpdate);
            PopulateAssignedRoleData(usuarioToUpdate);
            return View(usuarioToUpdate);
        }
        private void UpdateRolesUsuarios(string[] selectedRoles, Usuario usuarioToUpdate)
        {
            if (selectedRoles == null)
            {
                usuarioToUpdate.RolesUsuario = new List<RolesDeUsuarios>();
                return;
            }

            var selectedRolesHS = new HashSet<string>(selectedRoles);
            var usuariosRolesUsuarios = new HashSet<int>
                (usuarioToUpdate.RolesUsuario.Select(c => c.Rol.ID));
            foreach (var rol in _context.Roles)
            {
                if (selectedRolesHS.Contains(rol.ID.ToString()))
                {
                    if (!usuariosRolesUsuarios.Contains(rol.ID))
                    {
                        usuarioToUpdate.RolesUsuario.Add(new RolesDeUsuarios { UsuarioID = usuarioToUpdate.ID, RolID = rol.ID });
                    }
                }
                else
                {

                    if (usuariosRolesUsuarios.Contains(rol.ID))
                    {
                        RolesDeUsuarios rolesToRemove = usuarioToUpdate.RolesUsuario.FirstOrDefault(p => p.RolID == rol.ID);
                        _context.Remove(rolesToRemove);
                    }
                }
            }
        }
        // GET: Usuarios/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.ID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.ID == id);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "Administrador, Director, Cajero")]
        public async Task<IActionResult> Password(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            
            if (usuario == null)
            {
                return NotFound();
            }
            
            var changePassword = new ChangePassword();
            changePassword.ID = usuario.ID;
            changePassword.NombreUsuario = usuario.NombreUsuario;
            return View(changePassword);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador, Director, Cajero")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(int id, [Bind("ID,NombreUsuario,OldPassword,NewPassword,ConfirmarPassword")] ChangePassword usuarioPw)
        {
            if (id != usuarioPw.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (usuarioPw.NewPassword == usuarioPw.ConfirmarPassword)
                {
                    if(_context.Usuario.Any(u => u.NombreUsuario == usuarioPw.NombreUsuario))
                    {
                        var result = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                        var usuario = result;
                        if (HashHelper.CheckHash(usuarioPw.OldPassword, result.Password, result.Sal))
                        {
                            try
                            {

                                var hash = HashHelper.Hash(usuarioPw.NewPassword);
                                usuario.Password = hash.Password;
                                usuario.Sal = hash.Salt;
                                _context.Update(usuario);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                if (!UsuarioExists(usuario.ID))
                                {
                                    return NotFound();
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            await HttpContext.SignOutAsync();
                            return Redirect("/Login");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Contraseña incorrecta");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Usuario no encontrado");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden");
                }

            }

            return View(usuarioPw);
        }
    }
}
