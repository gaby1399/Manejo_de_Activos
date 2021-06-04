using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceAsegurado : IServiceAsegurado
    {
        public void DeleteAsegurado(int id)
        {
            IRepositoryAsegurado repository = new RepositoryAsegurado();
            repository.DeleteAsegurado(id);
        }

        public IEnumerable<Asegurado> GetAsegurado()
        {
            IRepositoryAsegurado repository = new RepositoryAsegurado();
            return repository.GetAsegurado();
        }

        public Asegurado GetAseguradoByID(int id)
        {
            IRepositoryAsegurado repository = new RepositoryAsegurado();
            return repository.GetAseguradoByID(id);
        }

        public bool Save(Asegurado asegurado)
        {
            IRepositoryAsegurado repository = new RepositoryAsegurado();
            return repository.Save(asegurado);
        }
    }
}
