using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public interface IRepositoryTipoActivo
    {
        IEnumerable<TipoActivo> GetTipoActivo();
        TipoActivo GetTipoActivoByID(int id);
        void DeleteTipoActivo(int id);
        bool Save(TipoActivo tipoActivo);
    }
}
