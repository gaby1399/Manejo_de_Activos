using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public interface IServiceMarca
    {
        IEnumerable<Marca> GetMarca();
        Marca GetMarcaByID(int id);
        void DeleteMarca(int id);
        bool Save(Marca marca);
        IEnumerable<Marca> GetMarcaByName(string name);
    }
}
