using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public interface IRepositoryAsegurado
    {
        IEnumerable<Asegurado> GetAsegurado();
        Asegurado GetAseguradoByID(int id);
        void DeleteAsegurado(int id);
        bool Save(Asegurado asegurado);
    }
}
