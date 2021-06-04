using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Utils;
using Web.ViewModels;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Login(ViewLogin usuario)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Usuario oUsuario = null;
            try
            {

                //validar model
                if (ModelState.IsValid)
                { 
                    //validar que existe y llamarlo
                    oUsuario = _ServiceUsuario.GetUsuario(usuario.loginName, Cryptography.EncrypthAES(usuario.contraseña));

                    if (oUsuario != null)
                    {//mantener al usuario activo
                        Session["User"] = oUsuario;
                        Log.Info($"Accede {oUsuario.nombre} {oUsuario.apellido} con el rol {oUsuario.Rol.idRol}-{oUsuario.Rol.descripcion}");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {//si da error
                        Log.Warn($"{usuario.loginName} se intentó conectar y falló");
                        TempData["Message"] = "Error al autenticarse";

                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult UnAuthorized()
        {
            try
            {
                ViewBag.Message = "No esta Auntenticado para esta pagina!";
                //valida al usuario 
                if (Session["User"] != null)
                {
                    Usuario oUsuario = Session["User"] as Usuario;
                    ViewBag.Message += ($"El usuario {oUsuario.nombre} {oUsuario.apellido} con el rol {oUsuario.Rol.idRol}-{oUsuario.Rol.descripcion}");

                    Log.Warn($"El usuario {oUsuario.nombre} {oUsuario.apellido} con el rol {oUsuario.Rol.idRol}-{oUsuario.Rol.descripcion}, intentó acceder una página sin derechos  ");
                }

                return View();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }


        public ActionResult Logout()
        {
            try
            {
                Log.Info("Se desconectó ");
                Session["User"] = null;
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }
    }
}
