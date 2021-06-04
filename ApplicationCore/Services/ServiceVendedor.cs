using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public class ServiceVendedor : IServiceVendedor
    {
        public void DeleteVendedor(string ced)
        {
            IRepositoryVendedor repository = new RepositoryVendedor();
            repository.DeleteVendedor(ced);
        }

        public IEnumerable<Vendedor> GetVendedor()
        {
            IRepositoryVendedor repository = new RepositoryVendedor();
            return repository.GetVendedor();
        }

        public Vendedor GetVendedorByID(string ced)
        {
            IRepositoryVendedor repository = new RepositoryVendedor();
           Vendedor vend= repository.GetVendedorByID(ced);
            return vend;
        }

        public IEnumerable<Vendedor> GetVendedorByName(string name)
        {
            IRepositoryVendedor repository = new RepositoryVendedor();
            return repository.GetVendedorByName(name);
        }

        public bool Save(Vendedor vendedor)
        {
            IRepositoryVendedor repository = new RepositoryVendedor();
            return repository.Save(vendedor);
        }
    }
}
