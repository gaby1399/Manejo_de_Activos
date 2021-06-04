using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Web.Utils;
using Web.ViewModels;

namespace Web.ApiControllers
{
    public class WebApiController : ApiController
    {
        [Route("WebApi/GetActivo")]
        [HttpGet]
        public IHttpActionResult GetActivo()
        {

            IEnumerable<Activo> lista = null;
            try
            {
                IServiceActivo service = new ServiceActivo();
                //manda una lista de activo
                lista = service.GetActivo();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());

                // Redireccion a la captura del Error
                return Ok("Error");
            }
        }



        // GET: api/WebApi
        public IHttpActionResult Get()
        {
            List<ViewAnalitico> lista = new List<ViewAnalitico>();
            ViewAnalitico viewAnalitico = new ViewAnalitico();
            viewAnalitico.Resultado = "Mensaje";

            for (int i = 0; i < 10; i++)
            {
                viewAnalitico.Resultado = "Mensaje" + DateTime.Now.ToString();
                lista.Add(viewAnalitico);
            }


            var str = new string[] { "value1", "value2" };
            return Ok(lista);
        }

        // GET: api/WebApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WebApi
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/WebApi/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/WebApi/5
        public void Delete(int id)
        {
        }
    }
}
