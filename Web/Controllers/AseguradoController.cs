using ApplicationCore.Services;
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
    public class AseguradoController : Controller
    {
        protected static String Action="";
        // GET: Asegurado
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Index()
        {
            try
            {
                return RedirectToAction("List");//llama al metodo List() y lo direcciona

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

        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult List()
        {
            IEnumerable<Asegurado> lista = null;
            try
            {
                Log.Info("Visita");//manda un mensaje de entrada para documentar en el log

                //hace una instancia del servidor y obtiene una lista de los vendedores
                IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();
                lista = _ServiceAsegurado.GetAsegurado();

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
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Save(Asegurado asegurado)
        {
            string errores = "";
            try
            {
                // Es valido
                if (ModelState.IsValid)
                {   //hace una instancia del servidor y actualiza o crea un vendedor
                    IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();
                    
                    if (_ServiceAsegurado.Save(asegurado))
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
                    return View("Create", asegurado);
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

        // GET: Vendedor/Details/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Details(int id)
        {
            IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();
            Asegurado asegurado = null;

            try
            {
                // Si va null
                if (id <= 0)
                {
                    ViewBag.Error = "Problemas a encontrar el codigo";
                    return View();
                }
                //obtiene el vendedor segun su cedula
                asegurado = _ServiceAsegurado.GetAseguradoByID(id);

                return PartialView("_DetalleAsegurado", asegurado);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                Action = "E";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Vendedor/Edit/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Edit(int id)
        {
            IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();
            Asegurado asg = null;
            try
            {
                // Si va null
                if (id <= 0)
                {
                    return RedirectToAction("List");
                }

                asg = _ServiceAsegurado.GetAseguradoByID(id);
                // Response.StatusCode = 500;
                return View(asg);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                Action = "E";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Vendedor/Create
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Vendedor/Delete/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Delete(int id)
        {
            try
            {
                // Si va null
                if (id <= 0)
                {
                    return RedirectToAction("List");
                }

                IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();
                Asegurado asg = _ServiceAsegurado.GetAseguradoByID(id);

                return View(asg);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                Action = "E";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult DeleteConfirmed(int id)
        {
            IServiceAsegurado _ServiceAsegurado = new ServiceAsegurado();

            try
            {

                if (id <= 0)
                {
                    return View();
                }

                List<Activo> listaAct = new List<Activo>();
                IServiceActivo service = new ServiceActivo();
                listaAct = (List<Activo>)service.GetActivo();

                if (listaAct != null)
                {
                    foreach (Activo act in listaAct)
                    {
                        if (act.idAsegurado == id)
                        {
                            TempData["Message"] = "No se puede eliminar, el asegurado ha sido asignado a un activo";
                            TempData.Keep();
                            Action = "E";
                            return RedirectToAction("List");
                        }
                    }

                }

                _ServiceAsegurado.DeleteAsegurado(id);
                Action = "D";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                Action = "E";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [ChildActionOnly]
        public ActionResult GetAseguradoActById(int id)
        {
            IServiceAsegurado service = new ServiceAsegurado();
            Asegurado oAsg = service.GetAseguradoByID(id);

            return Content(oAsg.idAsegurado + " - "+oAsg.descripcion);
        }
    }
}
