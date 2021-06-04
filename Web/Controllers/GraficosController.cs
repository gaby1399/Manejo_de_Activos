using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Security;

namespace Web.Controllers
{
    public class GraficosController : Controller
    {
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult ConsultarGrafico()
        {
            IServiceTipoActivo serviceT = new ServiceTipoActivo();
            ViewBag.Lista = serviceT.GetTipoActivo();
            return View();
        }
        public ActionResult ConsultaGraficoPorTipo(TipoActivo tipoA)
        {
            IServiceActivo service = new ServiceActivo();
            var lista = service.GetActivo();
            string descActivo = "";
            string costoActual = "";
            decimal costo = 0;
            foreach (var item in lista)
            {
                if (item.idTipoActivo==tipoA.idTipoActivo)
                {
                    // Hay que concatenarle comillas para datos string...
                    descActivo += "'" + item.descripcion + "',";
                   costo = (decimal)item.precioActual;
                    costoActual += costo.ToString() + ",";
                }
              
                
            }
            descActivo = descActivo.Substring(0, descActivo.Length - 1); // ultima coma
            costoActual = costoActual.Substring(0, costoActual.Length - 1);

            var colors = GenerateColors(lista.Count());
            // toma la lista y le agrega separa por comas (,)
            ViewBag.Color = string.Join(",", colors.ToList());
            ViewBag.ActTipo = descActivo;
            ViewBag.Precio = costoActual;
            ViewBag.Tipo = tipoA.descripcion;
          
            return PartialView("GraficoActivoTipo");
        }


        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult GraficoActivoTipo()
        {
            return View();
        }
      /*  public ActionResult GraficoActivoTipo()
        {
            IServiceTipoActivo serviceT = new ServiceTipoActivo();
            var listaTipo = serviceT.GetTipoActivo();
            string descTipoActivo = "";
            string costoActual = "";

            foreach (var tipo in listaTipo)
            { 
                // Hay que concatenarle comillas 'arduino','xxx', ...
                descTipoActivo += "'" + tipo.descripcion + "',";
                IServiceActivo service = new ServiceActivo();
                var lista = service.GetActivo();
                decimal precio=0;
                foreach (var act in lista)
                {
                    if (tipo.idTipoActivo==act.idTipoActivo)
                    {
                        precio += (decimal)act.precioActual;
                    }
                }
                decimal costo = precio;
                costoActual += costo.ToString() + ",";
            }
            descTipoActivo = descTipoActivo.Substring(0, descTipoActivo.Length - 1); // ultima coma
            costoActual = costoActual.Substring(0, costoActual.Length - 1);

            var colors = GenerateColors(listaTipo.Count());
            // toma la lista y le agrega separa por comas (,)
            ViewBag.Colores = string.Join(",", colors.ToList());
            ViewBag.Activo = descTipoActivo;
            ViewBag.Costo = costoActual;
            return View();
        }*/


        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        private List<string> GenerateColors(int pCantidad)
        {
            int numColors = pCantidad;
            var colors = new List<string>();
            var random = new Random(); // Make sure this is out of the loop!
            for (int i = 0; i < numColors; i++)
            {
                colors.Add(String.Format("'#{0:X6}'", random.Next(0x1000000)));
            }

            return colors;
        }


        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult AnaliticoActivo()
        {
            return View();
        }

    }
}
