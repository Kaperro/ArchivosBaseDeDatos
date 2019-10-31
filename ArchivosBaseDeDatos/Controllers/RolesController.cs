using ArchivosBaseDeDatos.Areas.Identity.Data;
using ArchivosBaseDeDatos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<GestorUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<GestorUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            var assignableRoles = _roleManager.Roles.ToList();
            return View(assignableRoles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleModel roleModel)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleModel.Name);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Roles/Assign
        public async Task<ActionResult> Assign()
        {
            var assignableRoles = _roleManager.Roles.ToList();
            ViewData["Name"] = new SelectList(assignableRoles, "Name", "Name");
            ViewData["UserName"] = new SelectList(_userManager.Users, "UserName", "UserName");
            return View(new RoleModel());
        }

        // POST: Roles/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(roleModel.UserName);
                if (user != null)
                {
                    if (await _roleManager.RoleExistsAsync(roleModel.Name))
                    {
                        if (await _userManager.IsInRoleAsync(user, roleModel.Name))
                        {
                            ViewData["Message"] = $@"Usuario {roleModel.UserName} ya posee el rol de {roleModel.Name}";
                            return View("Info");
                        }
                        else
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, roles);
                            await _userManager.AddToRoleAsync(user, roleModel.Name);
                            ViewData["Message"] = $@"Usuario {roleModel.UserName} ahora posee el rol de {roleModel.Name}";
                            return View("Info");
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Petición Invalida";
                        return View("Info");
                    }
                }
                else
                {
                    ViewData["Message"] = "Petición Invalida";
                    return View("Info");
                }
            }
            return View(roleModel);
        }
    }
}
