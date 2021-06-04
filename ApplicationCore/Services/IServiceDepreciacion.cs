using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public interface IServiceDepreciacion
    {
        IEnumerable<HistorialDepreciacion> GetDepreciacionByID(int id);
        IEnumerable<HistorialDepreciacion> GetDepreciacion();
        IEnumerable<HistorialDepreciacion> GetDepreciacionByValor(DateTime date);
        // decimal CalcularDepreciacion(Activo activo);
        void Save(HistorialDepreciacion depreciacion);
        //  void Save(HistorialDepreciacion depreciacion, decimal dolar);
        IEnumerable<HistorialDepreciacion> CalcularDepreciacion(List<Activo> listActivo);
        HistorialDepreciacion GetDepreciacionUltimoByID(int id);
    }
}
