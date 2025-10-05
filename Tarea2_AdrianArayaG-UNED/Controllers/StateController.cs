using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea2_AdrianArayaG_UNED.ViewModels;

namespace Tarea2_AdrianArayaG_UNED.Controllers
{
    public class StateController : Controller
    {
        [HttpPost]
        public ActionResult SaveFilter(FilterStateVm vm)
        {
            Session["FilterState"] = vm; return Json(new { ok = true });
        }
        [HttpGet]
        public ActionResult GetFilter()
        {
            var vm = Session["FilterState"] as FilterStateVm ?? new FilterStateVm();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}