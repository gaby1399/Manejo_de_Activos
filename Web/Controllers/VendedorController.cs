using ApplicationCore.Services;
using Infraestructure.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class VendedorController : Controller
    {
        protected static String Action;
        // GET: Vendedor
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
            IEnumerable<Vendedor> lista = null;
            try
            {
                Log.Info("Visita");//manda un mensaje de entrada para documentar en el log

                //hace una instancia del servidor y obtiene una lista de los vendedores
                IServiceVendedor _ServiceVendedor = new ServiceVendedor();
                lista = _ServiceVendedor.GetVendedor();

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
        public ActionResult Save(Vendedor vendedor)
        {
            string errores = "";
            try
            {
                // Es valido
                if (ModelState.IsValid)
                {   //hace una instancia del servidor y actualiza o crea un vendedor
                    Regex regex = new Regex(@"[^0-9]");
                    if (regex.IsMatch(vendedor.cedula))
                    {
                        ModelState.AddModelError("Cedula", "Cedula solo acepta valores numericos");
                        return View("Create", vendedor);
                    }
                    if (regex.IsMatch(vendedor.telefono))
                    {
                        ModelState.AddModelError("Telefono", "Telefono solo acepta valores numericos");
                        return View("Create", vendedor);
                    }
                    IServiceVendedor _ServiceVendedor = new ServiceVendedor();
                   if( _ServiceVendedor.Save(vendedor))
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
                    return View("Create", vendedor);
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
        public ActionResult Details(string ced)
        {
            IServiceVendedor _ServiceVendedor = new ServiceVendedor();
            Vendedor vendedor = null;

            try
            {
                // Si va null
                if (ced == null)
                {
                    ViewBag.Error = "Problemas a encontrar la cedula";
                    return View();
                }
                //obtiene el vendedor segun su cedula
                vendedor = _ServiceVendedor.GetVendedorByID(ced);

                return PartialView("_PartialViewDetailsVendedor", vendedor);
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
        public ActionResult Edit(string ced)
        {
            IServiceVendedor _ServiceVendedor = new ServiceVendedor();
            Vendedor vendedor = null;
            try
            {
                // Si va null
                if (ced == null)
                {
                    return RedirectToAction("List");
                }

                vendedor = _ServiceVendedor.GetVendedorByID(ced);
                // Response.StatusCode = 500;
                return View(vendedor);
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
        public ActionResult Delete(string ced)
        {
            try
            {
                // Si va null
                if (ced == null)
                {
                    return RedirectToAction("List");
                }

                IServiceVendedor _ServiceVendedor = new ServiceVendedor();
                Vendedor vendedor = _ServiceVendedor.GetVendedorByID(ced);
               
                return View(vendedor);
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
        public ActionResult DeleteConfirmed(string ced)
        {
            IServiceVendedor _ServiceVendedor = new ServiceVendedor();

            try
            {
                if (ced == null)
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
                        if (act.cedVendedor==ced)
                        {
                            TempData["Message"] = "No se puede eliminar, el vendedor ha sido asignado a un activo";
                            TempData.Keep();
                            Action = "E";
                            return RedirectToAction("List");
                        }
                    }

                }
                _ServiceVendedor.DeleteVendedor(ced);
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
        public ContentResult GetVendedorByName(string name)
        {
            IServiceVendedor _ServiceVendedor = new ServiceVendedor();
            var lista = _ServiceVendedor.GetVendedorByName(name).ToList();
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
        public ActionResult GetVendedorActById(string id)
        {
            IServiceVendedor service = new ServiceVendedor();
            Vendedor oVendedor = service.GetVendedorByID(id);

            return Content(oVendedor.cedula + " - " + oVendedor.nombre +" "+ oVendedor.apellido);
        }
    }
}
