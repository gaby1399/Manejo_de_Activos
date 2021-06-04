using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public interface IRepositoryMarca
    {
        IEnumerable<Marca> GetMarca();
        Marca GetMarcaByID(int id);
        void DeleteMarca(int id);
        bool Save(Marca marca);
        IEnumerable<Marca> GetMarcaByName(string name);
    }
}
