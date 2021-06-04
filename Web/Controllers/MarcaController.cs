using ApplicationCore.Services;
using Infraestructure.Models;
using Newtonsoft.Json;
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
    public class MarcaController : Controller
    {
        protected static String Action;
        // GET: Marca
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
            IEnumerable<Marca> lista = null;
            try
            {
                Log.Info("Visita");//manda un mensaje de entrada para documentar en el log

                //hace una instancia del servidor y obtiene una lista de las marcas
                IServiceMarca _ServiceMarca = new ServiceMarca();
                lista = _ServiceMarca.GetMarca();

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
        public ActionResult Save(Marca marca)
        {
            string errores = "";
            try
            {
                // Es valido
                if (ModelState.IsValid)
                {   //hace una instancia del servidor y actualiza o crea una marca
                    IServiceMarca _ServiceMarca = new ServiceMarca();
                    if (_ServiceMarca.Save(marca))
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
                    return View("Create", marca);
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

        // GET: Marca/Details/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Details(int id)
        {
            IServiceMarca _ServiceMarca = new ServiceMarca();
            Marca mar = null;

            try
            {
                // Si va null
                if (id <= 0)
                {
                    ViewBag.Error = "Problemas a encontrar el codigo";
                    return View();
                }
                //obtiene la marca según su id
                mar = _ServiceMarca.GetMarcaByID(id);

                return PartialView("_DetalleMarca", mar);
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

        // GET: Marca/Edit/5
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Edit(int id)
        {
            IServiceMarca _ServiceMarca = new ServiceMarca();
            Marca mar = null;
            try
            {
                // Si va null
                if (id <= 0)
                {
                    return RedirectToAction("List");
                }

                mar = _ServiceMarca.GetMarcaByID(id);
                // Response.StatusCode = 500;
                return View(mar);
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

        // GET: Marca/Create
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Marca/Delete/5
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

                IServiceMarca _ServiceMarca = new ServiceMarca();
                Marca mar = _ServiceMarca.GetMarcaByID(id);

                return View(mar);
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
            IServiceMarca _ServiceMarca = new ServiceMarca();

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
                        if (act.idMarca == id)
                        {
                            TempData["Message"] = "No se puede eliminar, la marca ha sido asignado a un activo";
                            TempData.Keep();
                            Action = "E";
                            return RedirectToAction("List");
                        }
                    }

                }

                _ServiceMarca.DeleteMarca(id);
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

        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ContentResult GetMarcaByName(string name)
        {
            IServiceMarca _ServiceMarca = new ServiceMarca();
            var lista = _ServiceMarca.GetMarcaByName(name).ToList();
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
            };
            string json = JsonConvert.SerializeObject(lista, settings);

            return Content(json);
        }

        //[CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        [ChildActionOnly]
        public ActionResult GetMarcaActById(int id)
        {
            IServiceMarca service = new ServiceMarca();
            Marca oMarca = service.GetMarcaByID(id);

            return Content(oMarca.idMarca + " - " + oMarca.descripcion);
        }
    }
}
