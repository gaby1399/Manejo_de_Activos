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
    public class TipoActivoController : Controller
    {
        protected static String Action;

        // GET: TipoActivo
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
            IEnumerable<TipoActivo> lista = null;
            try
            {
                Log.Info("Visita");//manda un mensaje de entrada para documentar en el log

                //hace una instancia del servidor y obtiene una lista de los vendedores
                IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();
                lista = _ServiceTipoActivo.GetTipoActivo();

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
        public ActionResult Save(TipoActivo tipo)
        {
            string errores = "";
            try
            {
                // Es valido
                if (ModelState.IsValid)
                {   //hace una instancia del servidor y actualiza o crea un vendedor
                    IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();
                    if (_ServiceTipoActivo.Save(tipo))
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
                    return View("Create", tipo);
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

        // GET: TipoActivo/Details/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Details(int id)
        {
            IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();
            TipoActivo tipo = null;

            try
            {
                // Si va null
                if (id <= 0)
                {
                    ViewBag.Error = "Problemas a encontrar el codigo";
                    return View();
                }
                //obtiene el vendedor segun su cedula
                tipo = _ServiceTipoActivo.GetTipoActivoByID(id);

                return PartialView("_DetalleTipoActivo", tipo);
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

        // GET: TipoActivo/Edit/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Edit(int id)
        {
            IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();
            TipoActivo tipo = null;
            try
            {
                // Si va null
                if (id <= 0)
                {
                    return RedirectToAction("List");
                }

                tipo = _ServiceTipoActivo.GetTipoActivoByID(id);
                // Response.StatusCode = 500;
                return View(tipo);
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

        // GET: TipoActivo/Create
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Create()
        {
            return View();
        }

        // GET: TipoActivo/Delete/5
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


                List<Activo> listaAct = new List<Activo>();
                IServiceActivo service = new ServiceActivo();
                listaAct = (List<Activo>)service.GetActivo();

                if (listaAct != null)
                {
                    foreach (Activo act in listaAct)
                    {
                        if (act.idTipoActivo == id)
                        {
                            TempData["Message"] = "No se puede eliminar, el tipo de activo ha sido asignado a un activo";
                            TempData.Keep();
                            Action = "E";
                            return RedirectToAction("List");
                        }
                    }

                }

                IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();
                TipoActivo tipo = _ServiceTipoActivo.GetTipoActivoByID(id);

                return View(tipo);
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
            IServiceTipoActivo _ServiceTipoActivo = new ServiceTipoActivo();

            try
            {

                if (id <= 0)
                {
                    return View();
                }

                _ServiceTipoActivo.DeleteTipoActivo(id);
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
        public ActionResult GetTipoActById(int id)
        {
            IServiceTipoActivo service = new ServiceTipoActivo();
            TipoActivo oTipo = service.GetTipoActivoByID(id);

            return Content(oTipo.idTipoActivo + " - " + oTipo.descripcion);
        }
    }
}
