//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infraestructure.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Infraestructure.Models.MetaData;

    [MetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario
    {
        public string loginName { get; set; }
        public int idRol { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public bool estado { get; set; }
    
        public virtual Rol Rol { get; set; }
    }
}
