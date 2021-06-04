using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceActivo : IServiceActivo
    {
        public void DeleteActivo(int id)
        {
            IRepositoryActivo repository = new RepositoryActivo();
            repository.DeleteActivo(id);
        }

        public IEnumerable<Activo> GetActivo()
        {
            IRepositoryActivo repository = new RepositoryActivo();
            return repository.GetActivo();
        }

        public IEnumerable<Activo> GetActivoByDate(string inicio, string final)
        {
            IRepositoryActivo repository = new RepositoryActivo();
            return repository.GetActivoByDate(inicio,final);
        }

        public Activo GetActivoByID(int id)
        {
            IRepositoryActivo repository = new RepositoryActivo();
           return repository.GetActivoByID(id);
        }

        public IEnumerable<Activo> GetActivoByName(string name)
        {
            IRepositoryActivo repository = new RepositoryActivo();
            return repository.GetActivoByName(name);
        }

        public bool Save(Activo activo)
        {
            IRepositoryActivo repository = new RepositoryActivo();
            return repository.Save(activo);
        }

        /*public void SaveDepreciacion(int id, decimal valor, decimal dolar)
        {
            IRepositoryActivo repository = new RepositoryActivo();
            repository.SaveDepreciacion(id,valor,dolar);
        }*/
    }
}
