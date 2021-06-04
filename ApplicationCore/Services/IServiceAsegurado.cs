using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public interface IServiceAsegurado
    {
        IEnumerable<Asegurado> GetAsegurado();
        Asegurado GetAseguradoByID(int id);
        void DeleteAsegurado(int id);
        bool Save(Asegurado asegurado);
    }
}
