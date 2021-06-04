using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public interface IServiceActivo
    {
        IEnumerable<Activo> GetActivo();
        IEnumerable<Activo> GetActivoByName(string name);
        IEnumerable<Activo> GetActivoByDate(string inicio, string final);
        Activo GetActivoByID(int id);
        void DeleteActivo(int id);
        bool Save(Activo activo);
       /* void SaveDepreciacion(int id, decimal valor, decimal dolar);*/
    }
}
