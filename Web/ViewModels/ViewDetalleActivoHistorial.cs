using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class ViewDetalleActivoHistorial
    {
    
        public Activo Activo { get; set; }
        public IEnumerable<HistorialDepreciacion> ListaDepreciacion { get; set; }
    }
}