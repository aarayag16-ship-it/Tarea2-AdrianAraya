using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tarea2_AdrianArayaG_UNED.Repositories;
using Tarea2_AdrianArayaG_UNED.Domain;
using Tarea2_AdrianArayaG_UNED.Services;

namespace Tarea2_AdrianArayaG_UNED.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _users = new UserRepository();


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }


        [ValidateAntiForgeryToken, HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) { ModelState.AddModelError("", "Credenciales requeridas"); return View(); }
            var u = _users.Find(userName);
            if (u == null || !CryptoService.Verify(password, u.Salt, u.PasswordHash)) { ModelState.AddModelError("", "Usuario o contraseña inválidos"); return View(); }
            FormsAuthentication.SetAuthCookie(u.UserName, true);
            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Tasks") : returnUrl);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register() => View();


        [ValidateAntiForgeryToken, HttpPost]
        [AllowAnonymous]
        public ActionResult Register(string userName, string displayName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(password))
            { ModelState.AddModelError("", "Todos los campos son obligatorios"); return View(); }
            if (_users.Exists(userName)) { ModelState.AddModelError("", "Usuario ya existe"); return View(); }
            var salt = Services.CryptoService.NewSalt();
            var u = new User { UserName = userName, DisplayName = displayName, Salt = salt, PasswordHash = Services.CryptoService.Hash(password, salt) };
            _users.Add(u);
            FormsAuthentication.SetAuthCookie(u.UserName, true);
            return RedirectToAction("Index", "Tasks");
        }


        [Authorize]
        public ActionResult Logout() { FormsAuthentication.SignOut(); return RedirectToAction("Login"); }
    }
}