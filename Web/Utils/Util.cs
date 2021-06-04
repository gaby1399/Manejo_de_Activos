using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Web.Utils
{

    public class Util
    {
        public static void ValidateErrors(Controller pController)
        {

            var listaErrores = pController.ModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();

            foreach (ModelErrorCollection item in listaErrores)
            {
                if (item.Count > 0)
                    pController.ModelState.AddModelError("", item[0].ErrorMessage.ToString());
            }

        }


        public static List<string> GetModelStateErrors(ModelStateDictionary pModelState)
        {

            List<string> lista = new List<string>();

            var listaErrores = pModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();

            foreach (var item in pModelState)
            {
                lista.Add(item.Value.Errors[0].ErrorMessage);
            }
            // pModelState.AddModelError("", "Name is required.");
            /*
            // Lo mismo que se hizo en Linq 
            foreach (var item in ModelState)
            {
                if (item.Value.Errors.Count > 0)
                {
                    foreach (var item2 in item.Value.Errors)
                    {
                        // ModelState.AddModelError("", item2.ErrorMessage);
                        errores = item2.ErrorMessage + " ";
                    }

                }
            }
            */

            return lista;

        }
    }
}