using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceUsuario
    {
        IEnumerable<Usuario> GetUsuario();
        Usuario GetUsuarioByID(string id);
        Usuario GetUsuario(string id, string password);
        void DeleteUsuario(string id);
        bool Save(Usuario usuario);
    }
}
