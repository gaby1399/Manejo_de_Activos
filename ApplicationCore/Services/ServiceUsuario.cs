using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        public void DeleteUsuario(string id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            repository.DeleteUsuario(id);
        }

        public IEnumerable<Usuario> GetUsuario()
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
           return repository.GetUsuario();
        }

        public Usuario GetUsuario(string id, string password)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
           return repository.GetUsuario(id,password);
        }

        public Usuario GetUsuarioByID(string id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
           return repository.GetUsuarioByID(id);
        }

        public bool Save(Usuario usuario)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.Save(usuario);
        }
    }
}
