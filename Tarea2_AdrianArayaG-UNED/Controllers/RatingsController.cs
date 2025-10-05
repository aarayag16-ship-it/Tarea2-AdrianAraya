using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea2_AdrianArayaG_UNED.Repositories;
using Tarea2_AdrianArayaG_UNED.ViewModels;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.Controllers
{
    [Authorize]
    public class RatingsController : Controller
    {
        private readonly TaskRepository _repo = new TaskRepository();


        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Create(RatingCreateVm vm)
        {
            if (!Guid.TryParse(vm.TaskId, out var gid)) return HttpNotFound();
            if (!ModelState.IsValid) return Json(new { ok = false, errors = ModelState });
            var rating = new Rating { Score = vm.Score, Comment = (vm.Comment ?? string.Empty), UserName = User.Identity.Name };
            _repo.AddRating(gid, rating);
            var t = _repo.Get(gid);
            return Json(new { ok = true, avg = t.AverageScore, count = t.Ratings.Count });
        }
    }
}