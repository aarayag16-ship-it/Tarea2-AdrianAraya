using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea2_AdrianArayaG_UNED.Repositories;


namespace Tarea2_AdrianArayaG_UNED.Filters
{
    public class AuthorOnlyAttribute : AuthorizeAttribute
    {
        private readonly TaskRepository _repo = new TaskRepository();
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var idStr = httpContext.Request.RequestContext.RouteData.Values["id"] as string;
            if (!Guid.TryParse(idStr, out var id)) return false;
            var task = _repo.Get(id);
            return task != null && httpContext.User.Identity.IsAuthenticated && task.AuthorUserName == httpContext.User.Identity.Name;
        }
    }
}