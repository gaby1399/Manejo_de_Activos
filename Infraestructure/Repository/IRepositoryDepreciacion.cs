using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public interface IRepositoryDepreciacion
    {
        IEnumerable<HistorialDepreciacion> GetDepreciacionByID(int id);
        IEnumerable<HistorialDepreciacion> GetDepreciacion();
        IEnumerable<HistorialDepreciacion> GetDepreciacionByValor(DateTime date);
        // decimal CalcularDepreciacion(Activo activo);
        IEnumerable<HistorialDepreciacion> CalcularDepreciacion(List<Activo> listActivo);
        void Save(HistorialDepreciacion depreciacion);
        //void Save(HistorialDepreciacion depreciacion, decimal dolar);
        HistorialDepreciacion GetDepreciacionUltimoByID(int id);


    }
}
