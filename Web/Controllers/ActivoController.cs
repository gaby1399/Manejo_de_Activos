using ApplicationCore.Services;
using Infraestructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class ActivoController : Controller
    {
        protected static String Action="";
        // GET: Activo
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
            IEnumerable<Activo> lista = null;
            try
            {
                Log.Info("Visita");//manda un mensaje de entrada para documentar en el log

                //hace una instancia del servidor y obtiene una lista de las marcas
                IServiceActivo _ServiceActivo = new ServiceActivo();
                lista = _ServiceActivo.GetActivo();

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
        public ActionResult Save(Activo activo, HttpPostedFileBase ImageFileFact, HttpPostedFileBase ImageFileA)
        {
            string errores = "";
            MemoryStream target = new MemoryStream();
            MemoryStream target1 = new MemoryStream();//ultimo cambio
            try
            {
                // Cuando es Insert Image viene en null porque se pasa diferente
               
                    if (ImageFileFact != null)
                    {
                        ImageFileFact.InputStream.CopyTo(target);
                        activo.fotoFactura = target.ToArray();
                        ModelState.Remove("ImageFileFact");
                    }
                    /*else
                    {
                        TempData["Message"] = "No ingreso una imagen de la Factura" + errores;
                        TempData.Keep();
                        return View("Create", activo);
                    }*/

                    if (ImageFileA != null)
                    {
                        ImageFileA.InputStream.CopyTo(target1);
                        activo.fotoActivo = target1.ToArray();
                        ModelState.Remove("ImageFileA");

                    }
                  /*  else
                    {
                        TempData["Message"] = "No ingreso una imagen del activo" + errores;
                        TempData.Keep();
                        return View("Create", activo);
                    }*/


             //   activo.idMarca = txtIdMarca;
                // Es valido
               if (ModelState.IsValid)
                {
                    //hace una instancia del servidor y actualiza o crea una marca
                    IServiceActivo _ServiceActivo = new ServiceActivo();
                  
                    if (activo.precioActual == null)
                         activo.precioActual = activo.precioColones;

                    ServiceBCCR service = new ServiceBCCR();
                        activo.precioDolares = service.GetDolar(activo.precioColones);
                  //  }
                  
                    if (_ServiceActivo.Save(activo))
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
                    return View("Create", activo);
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
            IServiceActivo _ServiceActivo = new ServiceActivo();
            Activo activo = null;

            try
            {
                // Si va null
                if (id <= 0)
                {
                    ViewBag.Error = "Problemas a encontrar el codigo";
                    return View();
                }
                //obtiene la marca según su id
                activo = _ServiceActivo.GetActivoByID(id);

                return PartialView("_DetalleActivo", activo);
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
            IServiceActivo _ServiceActivo = new ServiceActivo();
            Activo act = null;
            try
            {
                // Si va null
                if (id <= 0)
                {
                    return RedirectToAction("List");
                }
                IServiceAsegurado service = new ServiceAsegurado();
                ViewBag.ListaAsegurado = service.GetAsegurado();

                IServiceTipoActivo serviceT = new ServiceTipoActivo();
                ViewBag.ListaTipo = serviceT.GetTipoActivo();

                act = _ServiceActivo.GetActivoByID(id);

             //   int txtIdMarca = act.idMarca;
                // Response.StatusCode = 500;
                return View(act);
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
            IServiceAsegurado service = new ServiceAsegurado();
            ViewBag.ListaAsegurado= service.GetAsegurado();

            IServiceTipoActivo serviceT = new ServiceTipoActivo();
            ViewBag.ListaTipo = serviceT.GetTipoActivo();

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

                IServiceActivo _ServiceActivo = new ServiceActivo();
                Activo act = _ServiceActivo.GetActivoByID(id);

                return View(act);
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
            IServiceActivo _ServiceActivo = new ServiceActivo();

            try
            {

                if (id <=0)
                {
                    return View();
                }

                _ServiceActivo.DeleteActivo(id);
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


        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Reporte)]
        public ContentResult GetActivoByName(string name)
        {
            IServiceActivo _ServiceActivo = new ServiceActivo();
            var lista = _ServiceActivo.GetActivoByName(name).ToList();
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

        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        [ChildActionOnly]
        public ActionResult GetActById(int id)
        {
            IServiceActivo service = new ServiceActivo();
            Activo act = service.GetActivoByID(id);

            return Content(act.descripcion);
        }
    }
}
