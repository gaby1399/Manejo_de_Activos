using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public interface IRepositoryUsuario
    {
        IEnumerable<Usuario> GetUsuario();
        Usuario GetUsuarioByID(string id);
        void DeleteUsuario(string id);
        bool Save(Usuario usuario);
        Usuario GetUsuario(string id, string password);
    }
}
