using ApplicationCore.Services;
using Infraestructure.Models;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult ListaActivos()
        {
            IServiceActivo service = new ServiceActivo();
            IEnumerable<Activo> lista = service.GetActivo();
            return View(lista);
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult CrearPDFActivos()
        {
            IEnumerable<Activo> lista = null;
            try
            {
                // Extraer informacion
                IServiceActivo _ServiceActivo = new ServiceActivo();
                lista = _ServiceActivo.GetActivo();

                // Crear stream para almacenar en memoria el reporte 
                MemoryStream ms = new MemoryStream();
                //Initialize writer
                PdfWriter writer = new PdfWriter(ms);

                //Inicia el document
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document doc = new Document(pdfDoc);
                // Crea el header / Encabezado
                Paragraph header = new Paragraph("Lista de Activos")
                                   .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                   .SetFontSize(14)
                                   .SetFontColor(ColorConstants.BLUE);
                // Agrega el Encabezado
                doc.Add(header);

                // Crear tabla con 16 columnas 
                Table table = new Table(6, true);
             
                // los Encabezados
                table.AddHeaderCell("Activo");
                table.AddHeaderCell("Modelo");
                table.AddHeaderCell("Marca");
               // table.AddHeaderCell("Tipo de Activo");
               // table.AddHeaderCell("Precio en colones");
               // table.AddHeaderCell("Precio en dolares");
                table.AddHeaderCell("Precio Actual");
                //  table.AddHeaderCell("Fecha de vencimiento de garantia");
                //  table.AddHeaderCell("Fecha de vencimiento de seguro");
              //  table.AddHeaderCell("Condición del activo");
               // table.AddHeaderCell("Asegurado");
                table.AddHeaderCell("Imagen del Activo");
                table.AddHeaderCell("Factura");

                IServiceTipoActivo serviceTpAct = new ServiceTipoActivo();
                IServiceMarca serviceMarca = new ServiceMarca();
                IServiceAsegurado serviceAsegurado = new ServiceAsegurado();

                foreach (var item in lista)
                {
                    // Agregar datos a las celdas
                    table.AddCell(new Paragraph(item.descripcion.ToString()));
                    table.AddCell(new Paragraph(item.modelo.ToString()));
                    Marca marca =serviceMarca.GetMarcaByID(item.idMarca);
                    table.AddCell(new Paragraph(marca.descripcion.ToString()));
                    //TipoActivo tipo = serviceTpAct.GetTipoActivoByID(item.idTipoActivo);
                   // table.AddCell(new Paragraph(tipo.descripcion.ToString()));
                  //  table.AddCell(new Paragraph(item.precioColones.ToString()));
                   // table.AddCell(new Paragraph(item.precioDolares.ToString()));
                    table.AddCell(new Paragraph(item.precioActual.ToString()));
                    // table.AddCell(new Paragraph(item.fechaVenceGarantia.ToString()));
                    //table.AddCell(new Paragraph(item.fechaVenceSeguro.ToString()));
                   // table.AddCell(new Paragraph(item.estado.ToString()));
                   // Asegurado asegurado = serviceAsegurado.GetAseguradoByID(item.idAsegurado);
                   // table.AddCell(new Paragraph(asegurado.descripcion.ToString()));

                    // Convierte la imagen que viene en Bytes en imagen para PDF
                    Image imageAct = new Image(ImageDataFactory.Create(item.fotoActivo));
                    // Tamaño de la imagen
                    imageAct = imageAct.SetHeight(75).SetWidth(75);
                    table.AddCell(imageAct);

                    Image imageFact = new Image(ImageDataFactory.Create(item.fotoFactura));
                    // Tamaño de la imagen
                    imageFact = imageFact.SetHeight(75).SetWidth(75);
                    table.AddCell(imageFact);
                    // table.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    //     .SetFontSize(9);
                }
                doc.Add(table);

                // Calculo del monto total
                decimal montoTotal = (decimal)lista.ToList().Sum(k => k.precioActual);
                // Agrega  el monto total
                doc.Add(new Paragraph("\n\rMonto total de la depreciación de los activos: " + montoTotal.ToString("C", CultureInfo.CreateSpecificCulture("cr-CR"))));
                    
                /* doc.Add(new Paragraph("\n\rMonto total en dolares: " + montoTotalDolares.ToString(CultureInfo.CreateSpecificCulture("en-US"))))
                     .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                                    .SetFontSize(11);*/


                // Colocar número de páginas
               /* int numberOfPages = pdfDoc.GetNumberOfPages();
                for (int i = 1; i <= numberOfPages; i++)
                {

                    // Write aligned text to the specified by parameters point
                    doc.ShowTextAligned(new Paragraph(String.Format("pag {0} of {1}", i, numberOfPages)),
                            559, 826, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
                }*/


                //Close document
                doc.Close();

                // Retorna un File
                return File(ms.ToArray(), "application/pdf", "reporte");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult ConsultaVenceGarantia()
        {
            return View();
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult FiltarFechasGarantia(string txtFechaInicial, string txtFechaFinal)
        {
            try
            {
                if (txtFechaInicial == null || txtFechaFinal == null)
                {
                    TempData["Message"] = "Ingrese los datos determinados";
                    TempData.Keep();
                    return RedirectToAction("ConsultaVenceGarantia");
                }

                DateTime fechaI = DateTime.Parse(txtFechaInicial);
                DateTime fechaF = DateTime.Parse(txtFechaFinal);


                if (fechaI > fechaF)
                {
                    TempData["Message"] = "Sus datos no coinciden" ;
                    TempData.Keep();
                }

            IServiceActivo service = new ServiceActivo();
            IEnumerable<Activo> lista = service.GetActivoByDate(txtFechaInicial, txtFechaFinal);
                
                Log.Info("se realizo la consulta de fecha de vence garantia");

                //  return Json(lista, JsonRequestBehavior.AllowGet);
                return PartialView("_VenceGarantiaConsultado", lista);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult IndexConsultarActivo()
        {
            return View();
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult FiltrarActivo(int txtActivo)
        {
            try { 
            // Error porque viene en blanco 
            if (txtActivo<=0)
            {
                // Significa Error, va a ser capturado por onError del Javascript
                Response.StatusCode = -1;
                return View();
            }
           
            IServiceActivo serviceA = new ServiceActivo();
            Activo act = serviceA.GetActivoByID(txtActivo);
            IServiceDepreciacion serviceD = new ServiceDepreciacion();
            IEnumerable<HistorialDepreciacion> depreciacion = serviceD.GetDepreciacionByID(txtActivo);
            ViewDetalleActivoHistorial viewDetalle = new ViewDetalleActivoHistorial()
            {
                Activo = act,
                ListaDepreciacion = depreciacion,
            };
                Log.Info("se realizo la consulta de activo y depreciación");
            // Retorna un Partial View
            return PartialView("_ActivoConsultado", viewDetalle);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult _ActivoConsultado()
        {
            return View();
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult _VenceGarantiaConsultado()
        {
            return View();
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult ConsultarCierreDepreciacion()
        {
         
            return View();
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult FiltrarCierre(string txtFechaInicial)
        {
            try
            {
            if (txtFechaInicial == null)
            {
                TempData["Message"] = "Ingrese los datos determinados";
                TempData.Keep();
                return RedirectToAction("ConsultaVenceGarantia");
            }

            DateTime fechaI = DateTime.Parse(txtFechaInicial);
            IServiceDepreciacion service = new ServiceDepreciacion();
            List<HistorialDepreciacion> lista = new List<HistorialDepreciacion>();
            lista= (List<HistorialDepreciacion>)service.GetDepreciacionByValor(fechaI);

            if (lista==null)
            {
                TempData["Message"] = "No se encontro cierres de depreciación";
                TempData.Keep();
                return RedirectToAction("ConsultaVenceGarantia");
            }

            List<HistorialDepreciacion> listaCierre = new List<HistorialDepreciacion>();
            List<HistorialDepreciacion> listaC = new List<HistorialDepreciacion>();
                decimal valor = 0;
                for (int i=0; i<lista.Count;i++)
            {
                listaCierre = (List<HistorialDepreciacion>)service.GetDepreciacionByID(lista[i].idActivo);
                foreach (HistorialDepreciacion depreciacion in listaCierre)
                {
                        valor+=depreciacion.valor;
                        listaC.Add(depreciacion);
                }
            }

                if (listaC==null)
                {
                    TempData["Message"] = "No se encontro cierres de depreciación";
                    TempData.Keep();
                    return RedirectToAction("ConsultaVenceGarantia");
                }
               
                TempData["total"] = valor;

                Log.Info("Se encontraron cierres de depreciaciones");
                return View("_DetalleCierre", listaC);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Reporte)]
        public ActionResult _DetalleCierre()
        {
            return View();
        }
    }
}
