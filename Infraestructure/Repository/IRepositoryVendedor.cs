using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public interface IRepositoryVendedor
    {
        IEnumerable<Vendedor> GetVendedor();
        Vendedor GetVendedorByID(string ced);
        void DeleteVendedor(string ced);
        bool Save(Vendedor vendedor);
        IEnumerable<Vendedor> GetVendedorByName(string name);
    }
}
