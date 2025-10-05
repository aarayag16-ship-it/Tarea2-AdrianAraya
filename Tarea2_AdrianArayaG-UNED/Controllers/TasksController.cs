using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea2_AdrianArayaG_UNED.Repositories;
using Tarea2_AdrianArayaG_UNED.ViewModels;
using Tarea2_AdrianArayaG_UNED.Domain;
using Tarea2_AdrianArayaG_UNED.Services;

namespace Tarea2_AdrianArayaG_UNED.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskRepository _repo = new TaskRepository();
        private readonly TaskService _service = new TaskService();


        [HttpGet]
        public ActionResult Index()
        {
            var filter = Session["FilterState"] as FilterStateVm ?? new FilterStateVm { SortBy = "date" };
            var items = _service.ApplyFilter(_repo.All(), filter);
            return View(new TaskListVm { Items = items, Filter = filter });
        }


        [HttpGet]
        public ActionResult Details(string id)
        {
            if (!Guid.TryParse(id, out var gid)) return HttpNotFound();
            var it = _repo.Get(gid); if (it == null) return HttpNotFound();
            return View(it);
        }


        [Authorize]
        public ActionResult Create() => View(new TaskCreateVm());


        [Authorize, ValidateAntiForgeryToken, HttpPost]
        public ActionResult Create(TaskCreateVm vm, HttpPostedFileBase attachment)
        {
            // ========= PARCHE ROBUSTO: Normalizar Languages =========
            // Si el model binder no pobló Languages, intenta recuperarlo del form.
            if (vm.Languages == null || !vm.Languages.Any())
            {
                // Plan A: múltiples entradas con el mismo nombre "Languages"
                var langsMulti = Request.Form.GetValues("Languages");
                if (langsMulti != null && langsMulti.Length > 0)
                {
                    vm.Languages = langsMulti
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim())
                        .ToList();
                }

                // Plan B (fallback): una sola cadena CSV
                if (vm.Languages == null || !vm.Languages.Any())
                {
                    var csv = Request.Form["LanguagesCsv"]
                              ?? Request.Form["Languages"]
                              ?? Request.Form["langs"]; // por si usaste ese id en el input visible
                    if (!string.IsNullOrWhiteSpace(csv))
                    {
                        vm.Languages = csv.Split(',')
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .ToList();
                    }
                }

                // Si logramos poblar Languages, elimina el error previo y revalida el modelo
                if (vm.Languages != null && vm.Languages.Any())
                {
                    ModelState.Remove("Languages");
                    TryValidateModel(vm);
                }
            }
            // ========= FIN PARCHE =========

            if (!ModelState.IsValid)
            {
                var errs = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage);
                return Json(new { ok = false, errors = errs });
            }

            var t = new TaskItem
            {
                Title = vm.Title,
                Description = vm.Description,
                Languages = vm.Languages ?? new System.Collections.Generic.List<string>(),
                RepoUrl = vm.RepoUrl,
                Category = vm.Category,
                AuthorUserName = User.Identity.Name
            };

            if (attachment != null && attachment.ContentLength > 0)
            {
                var fileName = string.Format("{0}_{1}", t.Id, System.IO.Path.GetFileName(attachment.FileName));
                var savePath = Server.MapPath("~/App_Data/Uploads/" + fileName);
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savePath));
                attachment.SaveAs(savePath);
                t.AttachmentFileName = fileName;
            }

            _repo.Add(t);
            return Json(new { ok = true, id = t.Id.ToString(), redirect = Url.Action("Details", new { id = t.Id }) });
        }


        [Authorize]
        public ActionResult Edit(string id)
        {
            if (!Guid.TryParse(id, out var gid)) return HttpNotFound();
            var it = _repo.Get(gid); if (it == null) return HttpNotFound();
            if (it.AuthorUserName != User.Identity.Name) return new HttpUnauthorizedResult();
            return View(new TaskCreateVm { Title = it.Title, Description = it.Description, Languages = it.Languages, RepoUrl = it.RepoUrl, Category = it.Category });
        }


        [Authorize, ValidateAntiForgeryToken, HttpPost]
        public ActionResult Edit(string id, TaskCreateVm vm)
        {
            if (!Guid.TryParse(id, out var gid)) return HttpNotFound();
            if (!ModelState.IsValid) return Json(new { ok = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            var it = _repo.Get(gid); if (it == null) return HttpNotFound();
            if (it.AuthorUserName != User.Identity.Name) return new HttpUnauthorizedResult();
            it.Title = vm.Title; it.Description = vm.Description; it.Languages = vm.Languages ?? new System.Collections.Generic.List<string>();
            it.RepoUrl = vm.RepoUrl; it.Category = vm.Category; it.UpdatedAt = DateTime.UtcNow;
            _repo.Update(it);
            return Json(new { ok = true, redirect = Url.Action("Details", new { id = it.Id }) });
        }
    }
}