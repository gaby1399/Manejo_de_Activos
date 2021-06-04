using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceTipoActivo : IServiceTipoActivo
    {
        public void DeleteTipoActivo(int id)
        {
            IRepositoryTipoActivo repository = new RepositoryTipoActivo();
            repository.DeleteTipoActivo(id);
        }

        public IEnumerable<TipoActivo> GetTipoActivo()
        {
            IRepositoryTipoActivo repository = new RepositoryTipoActivo();
           return repository.GetTipoActivo();
        }

        public TipoActivo GetTipoActivoByID(int id)
        {
            IRepositoryTipoActivo repository = new RepositoryTipoActivo();
            return repository.GetTipoActivoByID(id);
        }

        public bool Save(TipoActivo tipoActivo)
        {
            IRepositoryTipoActivo repository = new RepositoryTipoActivo();
            return repository.Save(tipoActivo);
        }
    }
}
