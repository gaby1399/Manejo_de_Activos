using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly int[] allowedroles;
        public CustomAuthorizeAttribute(params int[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var oUsuario = (Usuario)httpContext.Session["User"];

            if (oUsuario != null)
            {

                foreach (var rol in allowedroles)
                {
                    if (rol == oUsuario.idRol)
                        return true;
                }
            }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Si hubo un error redireccione a el siguiente Controller y View
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Login" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}