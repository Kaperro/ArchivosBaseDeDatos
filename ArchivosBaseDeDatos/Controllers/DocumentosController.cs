using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArchivosBaseDeDatos.Models.Entities;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using ArchivosBaseDeDatos.Hubs;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ArchivosBaseDeDatos.Models;
using Microsoft.AspNetCore.Identity;
using ArchivosBaseDeDatos.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using ArchivosBaseDeDatos.Utils;

namespace ArchivosBaseDeDatos.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {
        private readonly GestorContext _context;
        private readonly IHubContext<MainHub> _hubContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<GestorUser> _userManager;

        public DocumentosController(GestorContext context,
            IHubContext<MainHub> hubContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<GestorUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Documentos
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            if(role.Count == 0)
            {
                ViewData["Message"] = $@"Usted no pertenece a ningun departamento.";
                return View("Info");
            }

            var data = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    ArchivoNombre = d.ArchivoNombre,
                    Departamento = d.Departamento,
                    Destinatario = d.Destinatario,
                    Usuario = d.Usuario,
                    FechaCreado = d.FechaCreado,
                    FechaRevisado = d.FechaRevisado,
                })
                .Where(d => d.Departamento == role.FirstOrDefault() || d.Destinatario == User.Identity.Name)
                .ToListAsync();
            ViewData["Departamento"] = role.FirstOrDefault();

            return View(data);
        }

        // GET: Documentos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // GET: Documentos/Create
        public async Task<IActionResult> Create()
        {
            var roles = new SelectList(await _roleManager.Roles.Where(r => r.Name != SystemRoles.Administrator).ToListAsync(),"Name","Name").ToList();
            roles.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Roles"] = roles;
            var users = new SelectList(await _userManager.Users.Where(r => r.UserName != User.Identity.Name).ToListAsync(),"UserName", "UserName").ToList();
            users.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Users"] = users;
            //ViewData["Roles"] = new SelectList(_roleManager.Roles.Where(r => r.Name != SystemRoles.Administrator), "Name", "Name");
            //ViewData["Users"] = new SelectList(_userManager.Users.Where(r => r.UserName != User.Identity.Name), "UserName", "UserName");
            return View();
        }

        // POST: Documentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Documento documento)
        {
            if (ModelState.IsValid)
            {
                if (documento.ArchivoHelper.Length > 0)
                {
                    documento.ArchivoNombre = documento.ArchivoHelper.FileName;
                    using (var ms = new MemoryStream())
                    {
                        documento.ArchivoHelper.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        documento.Archivo64 = s;
                    }
                }
                documento.FechaCreado = DateTime.Now;
                documento.Usuario = User.Identity.Name;
                _context.Add(documento);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("CheckGroupTray");
                return RedirectToAction(nameof(Index));
            }
            var roles = new SelectList(await _roleManager.Roles.Where(r => r.Name != SystemRoles.Administrator).ToListAsync(), "Name", "Name").ToList();
            roles.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Roles"] = roles;
            var users = new SelectList(await _userManager.Users.Where(r => r.UserName != User.Identity.Name).ToListAsync(), "UserName", "UserName").ToList();
            users.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Users"] = users;
            return View(documento);
        }

        // GET: Documentos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            return View(documento);
        }

        // POST: Documentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Nombre,Archivo,Archivo64")] Documento documento)
        {
            if (id != documento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentoExists(documento.Id))
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
            return View(documento);
        }

        // GET: Documentos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // POST: Documentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var documento = await _context.Documento.FindAsync(id);
            _context.Documento.Remove(documento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(long id)
        {
            return _context.Documento.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Download(long id)
        {
            var documento = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id,
                    ArchivoNombre = d.ArchivoNombre,
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            var file = await _context.Documento
                .Where(d => d.Id == id)
                .Select(d => d.Archivo64)
                .FirstOrDefaultAsync();
            if (documento == null)
            {
                return NotFound();
            }
            byte[] bytes = Convert.FromBase64String(file);
            return File(bytes, "multipart/form-data", documento.ArchivoNombre);
        }

        [HttpGet]
        public async Task<IActionResult> Check(long id)
        {
            var documento = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documento == null)
            {
                return NotFound();
            }
            documento.FechaRevisado = DateTime.Now;
            _context.Entry(documento).Property(x => x.FechaRevisado).IsModified = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RetrieveDataAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            if (role.Count == 0)
            {
                ViewData["Message"] = $@"Usted no pertenece a ningun departamento.";
                return View("Info");
            }

            var data = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    ArchivoNombre = d.ArchivoNombre,
                    Departamento = d.Departamento,
                    Destinatario = d.Destinatario,
                    Usuario = d.Usuario,
                    FechaCreado = d.FechaCreado,
                    FechaRevisado = d.FechaRevisado,
                })
                .Where(d => d.Departamento == role.FirstOrDefault() || d.Destinatario == User.Identity.Name)
                .ToListAsync();
            return new PartialViewResult()
            {
                ViewName = "_Index",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = data,
                }
            };
        }

        // GET: Documentos/Transfer
        public async Task<IActionResult> Transfer(long id)
        {
            var documento = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    Departamento = d.Departamento,
                    Usuario = d.Usuario,
                })
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
            var roles = new SelectList(await _roleManager.Roles.Where(r => r.Name != SystemRoles.Administrator).ToListAsync(), "Name", "Name").ToList();
            roles.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Roles"] = roles;
            var users = new SelectList(await _userManager.Users.Where(r => r.UserName != User.Identity.Name).ToListAsync(), "UserName", "UserName").ToList();
            users.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Users"] = users; var model = new DocumentoTransferViewModel
            { Id = documento.Id, Departamento = documento.Departamento, Nombre = documento.Nombre };
            return View(model);
        }

        // POST: Documentos/Transfer
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(Documento documento)
        {
            if (ModelState.IsValid)
            {
                var documentoInDb = await _context.Documento.FindAsync(documento.Id);
                var documentoRegistro =
                    new DocumentoRegistro
                    {
                        Documento = documentoInDb.Id,
                        Departamento = documentoInDb.Departamento,
                        Destinatario = documentoInDb.Destinatario,
                        TiempoInicio = documentoInDb.FechaCreado,
                        TiempoFin = DateTime.Now,
                        Usuario = User.Identity.Name
                    };
                documentoInDb.Departamento = documento.Departamento;
                _context.Entry(documentoInDb).Property(x => x.Departamento).IsModified = true;
                documentoInDb.Destinatario = documento.Destinatario;
                _context.Entry(documentoInDb).Property(x => x.Destinatario).IsModified = true;
                await _context.SaveChangesAsync();
                _context.Add(documentoRegistro);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("CheckGroupTray");
                return RedirectToAction(nameof(Index));
            }
            var roles = new SelectList(await _roleManager.Roles.Where(r => r.Name != SystemRoles.Administrator).ToListAsync(), "Name", "Name").ToList();
            roles.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Roles"] = roles;
            var users = new SelectList(await _userManager.Users.Where(r => r.UserName != User.Identity.Name).ToListAsync(), "UserName", "UserName").ToList();
            users.Insert(0, new SelectListItem("Seleccione", "", true, true));
            ViewData["Users"] = users;
            return View(documento);
        }

        public async Task<IActionResult> Log(long id)
        {
            var model = await _context.DocumentoRegistro
                .Where(x => x.Documento == id)
                .ToListAsync();
            var documento = await _context.Documento
                .Select(d => new Documento
                {
                    Id = d.Id,
                    Nombre = d.Nombre
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            @ViewData["Documento"] = $"{documento.Nombre} {documento.Id} ";
            return View(model);
        }
    }
}
