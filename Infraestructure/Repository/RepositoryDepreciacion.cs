using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryDepreciacion : IRepositoryDepreciacion
    {
        IEnumerable<HistorialDepreciacion> IRepositoryDepreciacion.CalcularDepreciacion(List<Activo> listActivo)
        {
            try { 
            List<HistorialDepreciacion> historial =new List<HistorialDepreciacion>();

            foreach (Activo act in listActivo)
            {//Busca en la lista de activos enviada ,los activos para depreciar, de tal forma que si 
             //tiene más de un mes sin depreciar se realice el calculo y si aun no acumplido el mes no se deprecia.
                if ((act.fechaCompra.Month < DateTime.Now.Month || act.fechaCompra.Year < DateTime.Now.Year) && act.precioActual!=0) //ultimo
                {
                        decimal depre = 0;
                        decimal precio = (decimal)act.precioActual;
                        int vida = act.vidaUtil;
                        DateTime fecha = new DateTime();
                        HistorialDepreciacion depreciacion = new HistorialDepreciacion();
                        if (act.precioColones == precio)
                        {
                            fecha = act.fechaCompra;
                            
                            int cantidadMes; //ultimo
                            if(fecha.Year < DateTime.Now.Year )
                               cantidadMes =(12-act.fechaCompra.Month)+DateTime.Now.Month;
                            else
                                cantidadMes = DateTime.Now.Month - act.fechaCompra.Month;

                            for (int i = 1; i <= cantidadMes; i++) //ultimo
                            {
                                HistorialDepreciacion odepreciacion = new HistorialDepreciacion();
                                depre = Calcular(act.precioColones, vida);
                                odepreciacion.idActivo = act.idActivo;
                                odepreciacion.valor = precio - depre;
                                precio = odepreciacion.valor;//ultimo
                                odepreciacion.Fecha = AgregarFecha(fecha);
                                fecha = odepreciacion.Fecha;
                                historial.Add(odepreciacion);
                               // depreciacion = null;
                            }
                          
                        }
                        else
                        {
                            depre = Calcular(act.precioColones, vida);
                            depreciacion.idActivo = act.idActivo;
                            depreciacion.valor = precio - depre;
                            foreach (HistorialDepreciacion hist in GetDepreciacionByID(act.idActivo))
                            {
                                if (hist.valor == act.precioActual)
                                {
                                    fecha = hist.Fecha;
                                }
                            }
                            depreciacion.Fecha = AgregarFecha(fecha);
                            historial.Add(depreciacion);
                        }

                    }
                
            }
                return historial;
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }

        }

        public DateTime AgregarFecha(DateTime fecha)
        {
            int mes = fecha.Month;
            if (mes + 1 > 12)
            {

               return fecha.AddDays(+30).AddMonths(0).AddYears(0);
            }
            else
            {
                return fecha.AddDays(+30).AddMonths(0);
            }

        }

        public decimal Calcular(decimal precioActual,int vidaUtil) //ultimo
        {
            decimal precio = precioActual;
            int utilidad = vidaUtil;
            int periodo = utilidad * 12;

            return precio / periodo;
        }


        public IEnumerable<HistorialDepreciacion> GetDepreciacionByID(int id)
        {
            IEnumerable<HistorialDepreciacion> lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.HistorialDepreciacion.ToList<HistorialDepreciacion>().FindAll(p => p.idActivo == id);
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public HistorialDepreciacion GetDepreciacionUltimoByID(int id)
        {
           HistorialDepreciacion lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.HistorialDepreciacion.ToList<HistorialDepreciacion>().FindLast(p => p.idActivo == id);
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public void Save(HistorialDepreciacion depreciacion)
        {
            int retorno = 0;
            try {
                using (MyContext ctx = new MyContext()) 
                {
                using (var Transaction = ctx.Database.BeginTransaction())
                {
                 try {  
                    ctx.HistorialDepreciacion.Add(depreciacion);
                    retorno = ctx.SaveChanges();

                    ctx.Database.ExecuteSqlCommand(
                       "Update Activo set precioActual=" + depreciacion.valor + " where idActivo ="
                       + depreciacion.idActivo) ;
                     retorno = ctx.SaveChanges();

                    Transaction.Commit();
                   }
                        catch (Exception ex)
                        {
                            Transaction.Rollback();
                            string mensaje = "";
                            Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                            throw new Exception(mensaje);
                        }
                    }
                }   
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        /*public void Save(HistorialDepreciacion depreciacion)
        {
            int retorno = 0;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                ctx.Configuration.LazyLoadingEnabled = false;
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ctx.HistorialDepreciacion.Add(depreciacion);
                        retorno = ctx.SaveChanges();


                        Activo modif = ctx.Activo.Find(depreciacion.idActivo);
                         retorno = ctx.SaveChanges();

                        if (modif != null)
                        {
                //  string sql = string.Format("Update Activo set precioActual=" + depreciacion.valor + " where idActivo =" + depreciacion.idActivo);
                                modif.precioActual = depreciacion.valor;
                                retorno = ctx.SaveChanges();
                               // transaction.Commit();
                            }
                           
                        }
                    catch (Exception ex)
                    {
                       // transaction.Rollback();
                        string mensaje = "";
                        Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                        throw new Exception(mensaje);
                    }
                }
             }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }*/

        public IEnumerable<HistorialDepreciacion> GetDepreciacion()
        {
            IEnumerable<HistorialDepreciacion> lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.HistorialDepreciacion.ToList<HistorialDepreciacion>();
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<HistorialDepreciacion> GetDepreciacionByValor(DateTime date)
        {
            IEnumerable<HistorialDepreciacion> lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.HistorialDepreciacion.ToList<HistorialDepreciacion>().FindAll(p => p.valor == 0 && p.Fecha==date);
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

    }
}
