using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceRol : IServiceRol
    {
  
        IEnumerable<Rol> IServiceRol.GetRol()
        {
            IRepositoryRol repositoryRol = new RepositoryRol();
            return repositoryRol.GetRol();
        }
    }
}
