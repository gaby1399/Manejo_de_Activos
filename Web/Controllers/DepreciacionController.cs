using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class DepreciacionController : Controller
    {
        // GET: Depreciacion
        public ActionResult _DetalleDepreciacion()
        {
            return View();
        }

        public ActionResult _ErrorDepreciacion()
        {
            return View();
        }

        // GET: Depreciacion/Create
        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Create()
        {
          
            return View();
        }

        [CustomAuthorizeAttribute((int)Roles.Administrador, (int)Roles.Proceso)]
        public ActionResult Save()
        {
            try
            {
            IServiceDepreciacion service = new ServiceDepreciacion();
           
            int mes = DateTime.Now.Month;
            int año= DateTime.Now.Year;
             List<Activo> ListActivo = new List<Activo>();
                //valida que no se haya hecho la depreciacion en otro momento en un mismo periodo
                IServiceActivo serviceAct = new ServiceActivo();
                IEnumerable<Activo> oActivoList = serviceAct.GetActivo();

            foreach (Activo act in oActivoList)
            {
                    // foreach (HistorialDepreciacion historialDepre in listaHistorial)
                    // {
                    HistorialDepreciacion depre = service.GetDepreciacionUltimoByID(act.idActivo);
                    if (depre==null)
                    {
                        ListActivo.Add(act);
                    }
                    else { 
                            if (depre.Fecha.Month < mes || depre.Fecha.Year < año)
                            {
                                ListActivo.Add(act);
                            }
                    }
                    // }
                }

                if (ListActivo == null)
                {
                    TempData["Message"] = "No se encontraron activos para depreciar";
                    TempData.Keep();
                    return PartialView("_ErrorDepreciacion");
                }

                List<HistorialDepreciacion> historial = (List<HistorialDepreciacion>)service.CalcularDepreciacion(ListActivo);

                if (historial.Count!=0)
                {
                    foreach (HistorialDepreciacion depreciacion in historial)
                    {
                        service.Save(depreciacion);
                    }

                    Log.Info("Depreciación realizada");

                    TempData["Save"] = "La depreciación se ha realizado";
                    TempData.Keep();

                    return PartialView("_DetalleDepreciacion", historial);

                }
                else { 

                TempData["Message"] = "No se encontraron los activos";
                TempData.Keep();
                return PartialView("_ErrorDepreciacion");
                }
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
    }
}
