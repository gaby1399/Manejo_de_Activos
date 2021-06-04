using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceDepreciacion : IServiceDepreciacion
    {
      /*  public decimal CalcularDepreciacion(Activo activo)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
           return repository.CalcularDepreciacion(activo);
        }*/

        public IEnumerable<HistorialDepreciacion> CalcularDepreciacion(List<Activo> listActivo)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
            return repository.CalcularDepreciacion(listActivo);
        }

        public IEnumerable<HistorialDepreciacion> GetDepreciacion()
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
            return repository.GetDepreciacion();
        }

        public IEnumerable<HistorialDepreciacion> GetDepreciacionByID(int id)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
            return repository.GetDepreciacionByID(id);
        }

        public IEnumerable<HistorialDepreciacion> GetDepreciacionByValor(DateTime date)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
            return repository.GetDepreciacionByValor(date);
        }

        public HistorialDepreciacion GetDepreciacionUltimoByID(int id)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
            return repository.GetDepreciacionUltimoByID(id);
        }

        public void Save(HistorialDepreciacion depreciacion)
        {
            IRepositoryDepreciacion repository = new RepositoryDepreciacion();
             repository.Save(depreciacion);
        }

    }
}
