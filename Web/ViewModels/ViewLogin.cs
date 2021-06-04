using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class ViewLogin
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Debe ingresar el usuario")]
        public string loginName { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        public string contraseña { get; set; }
       
    }
}