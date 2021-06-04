using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class ViewDetalleDepreciacion
    {
        [Display(Name ="Nombre del Activo")]
        public string NombreActivo { get; set; }
        public decimal Depreciación { get; set; }
        [Display(Name = "Precio Actual")]

        public decimal PrecioActual { get; set; }
    }
}