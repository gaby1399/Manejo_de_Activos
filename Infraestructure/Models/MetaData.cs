using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models
{
   public class MetaData
    {// [MetadataType(typeof(ActivoMetadata))]
        internal partial class ActivoMetadata
        {
            [Display(Name = "Codigo")]
            public int idActivo { get; set; }
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            [Display(Name = "Número de serie")]
            public int numSerie { get; set; }
            [Display(Name = "Tipo Activo")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public int idTipoActivo { get; set; }
            [Display(Name = "Modelo")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            [StringLength(15,ErrorMessage ="Su rango maximo de letras es 15")]
            public string modelo { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
            [Display(Name = "Fecha de la compra")]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public System.DateTime fechaCompra { get; set; }

            [Display(Name = "Precio en Colones")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public decimal precioColones { get; set; }

            [Display(Name = "Precio en Dolares")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public decimal precioDolares { get; set; }
            [Display(Name = "Descripción")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            public string descripcion { get; set; }
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Fecha que vence la garantia")]
            [DataType(DataType.Date)]
            public System.DateTime fechaVenceGarantia { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Fecha que vence el seguro")]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public System.DateTime fechaVenceSeguro { get; set; }
            [Display(Name = "Condición del activo")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public string estado { get; set; }
            [Display(Name = "Fotografia de factura")]
            public byte[] fotoFactura { get; set; }
            [Display(Name = "Fotografia de activo")]
            public byte[] fotoActivo { get; set; }
            [Display(Name = "Marca")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public int idMarca { get; set; }
            [Display(Name = "Asegurado")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public int idAsegurado { get; set; }
            [Display(Name = "Vendedor")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public string cedVendedor { get; set; }
            [Display(Name = "Vida util")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public int vidaUtil { get; set; }
        }

        internal partial class AseguradoMetadata
        {
            [Display(Name = "Asegurado")]
            public int idAsegurado { get; set; }
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            [Display(Name = "Descripción")]
            public string descripcion { get; set; }

        }
        internal partial class HistorialMetadata
        {
            public int idDepreciacion { get; set; }
            [Display(Name = "Codigo de Activo")]
            public int idActivo { get; set; }
            [Display(Name = "Precio actual")]
            public decimal valor { get; set; }
            public System.DateTime Fecha { get; set; }

        }
        internal partial class MarcaMetadata
        {
            [Display(Name = "Codigo Marca")]
            public int idMarca { get; set; }
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            [Display(Name = "Descripción")]
            [Required(ErrorMessage = "El {0} es un dato requerido")]
            public string descripcion { get; set; }


        }
        internal partial class TipoActivoMetadata
        {
            [Display(Name = "Codigo Tipo Activo")]
            public int idTipoActivo { get; set; }
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            [Display(Name = "Descripción")]
            public string descripcion { get; set; }

        }
        internal partial class UsuarioMetadata
        {
            [Display(Name = "Usuario")]
            [Required(ErrorMessage = "Debe ingresar el usuario")]
            [StringLength(20, ErrorMessage = "Su rango maximo de letras es 20")]
            public string loginName { get; set; }
            [Display(Name = "Rol")]
            [Required(ErrorMessage = "Escoja un Rol")]
            public int idRol { get; set; }
            [Display(Name = "Contraseña")]
            [Required(ErrorMessage = "Debe ingresar la contraseña")]
            public string contraseña { get; set; }
            [Display(Name = "Nombre")]
            [StringLength(20, ErrorMessage = "Su rango maximo de letras es 20")]
            [Required(ErrorMessage = "Debe ingresar un nombre")]
            public string nombre { get; set; }
            [Required(ErrorMessage = "Debe ingresar un apellido")]
            [Display(Name = "Apellido")]
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            public string apellido { get; set; }
            [Display(Name = "Activo")]
            public bool estado { get; set; }

        }

        internal partial class VendedorMetadata
        {
            [Display(Name = "Cedula")]
            [StringLength(15, ErrorMessage = "La cedula debe ser minimo de 9 digitos", ErrorMessageResourceName = "", ErrorMessageResourceType = null, MinimumLength = 9)]
            public string cedula { get; set; }
            [Display(Name = "Nombre")]
            [StringLength(10, ErrorMessage = "Su rango maximo de letras es 10")]
            [Required(AllowEmptyStrings = true, ErrorMessage = "Ingrese su Nombre")]
            public string nombre { get; set; }
            [Display(Name = "Apeliido")]
            [StringLength(10, ErrorMessage = "Su rango maximo de letras es 10")]
            [Required(AllowEmptyStrings = true, ErrorMessage = "Ingrese su Apellido")]
            public string apellido { get; set; }
            [Display(Name = "Dirección")]
            [StringLength(30, ErrorMessage = "Su rango maximo de letras es 30")]
            [Required(AllowEmptyStrings = true, ErrorMessage = "Ingrese su Dirección")]
            public string direccion { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Telefóno")]
            [StringLength(12, ErrorMessage = "El telefóno debe ser minimo de 8 digitos", ErrorMessageResourceName = "", ErrorMessageResourceType = null, MinimumLength = 8)]
            public string telefono { get; set; }

        }

    }
}

