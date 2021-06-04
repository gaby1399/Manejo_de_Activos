using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        protected static String Action = "";
        // GET: Usuario
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Index()
        {
            try
            {
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult List()
        {
            IEnumerable<Usuario> lista = null;
            try
            {
                Log.Info("Visita");


                IServiceUsuario _ServiceUsuario = new ServiceUsuario();
                lista = _ServiceUsuario.GetUsuario();
                if (!String.IsNullOrEmpty(Action))
                {
                    ViewBag.Action = Action;

                }
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());

                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Save(Usuario usuario)
        {
            string errores = "";
            try
            {
                // Es valido
                if (ModelState.IsValid)
                {
                    ServiceUsuario _ServiceUsuario = new ServiceUsuario();
                   string pass= Cryptography.EncrypthAES(usuario.contraseña);
                    usuario.contraseña = pass;

                    if (_ServiceUsuario.Save(usuario))
                        Action = "S";
                    else
                        Action = "U";
                
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Util.ValidateErrors(this);

                    TempData["Message"] = "Error al procesar los datos! " + errores;
                    TempData.Keep();

                    return View("Create", usuario);
                }

                // redirigir
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }


        // GET: Usuario/Details/5      
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Details(string id)
        {
            ServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Usuario usuario = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("List");
                }

                usuario = _ServiceUsuario.GetUsuarioByID(id);

                return PartialView("_DetalleUsuario", usuario);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Usuario/Edit/5
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Edit(string id)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Usuario usuario = null;
            IServiceRol serviceRol = new ServiceRol();
            List<Rol> listaRol = (List<Rol>)serviceRol.GetRol();
            ViewBag.ListaRol = listaRol;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("List");
                }

                usuario = _ServiceUsuario.GetUsuarioByID(id);
                // Response.StatusCode = 500;
                return View(usuario);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }


        // GET: Usuario/Create
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Create()
        {
            IServiceRol serviceRol = new ServiceRol();
            List<Rol> listaRol = (List<Rol>)serviceRol.GetRol();
            ViewBag.ListaRol = listaRol;
            return View();
        }


        // GET: Bodega/Delete/5
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult Delete(string id)
        {
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("List");
                }

                ServiceUsuario _ServiceUsuario = new ServiceUsuario();
                Usuario usuario = _ServiceUsuario.GetUsuarioByID(id);

                return View(usuario);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAttribute((int)Roles.Administrador)]
        public ActionResult DeleteConfirmed(string id)
        {
            ServiceUsuario _ServiceUsuario = new ServiceUsuario();

            try
            {

                if (id == null)
                {
                    return View();
                }

                _ServiceUsuario.DeleteUsuario(id);
                Action = "D";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
    }
}
