using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public class RepositoryActivo : IRepositoryActivo
    {
        public void DeleteActivo(int id)
        {
            int returno;
            try
            {
             using (MyContext ctx = new MyContext())
                {

              using (var Transaction = ctx.Database.BeginTransaction())
                {
                try
                  {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    ctx.Database.ExecuteSqlCommand("Delete from historialDepreciacion where idActivo="
                       + id);
                    returno = ctx.SaveChanges();

                    Activo act = new Activo()
                    {
                        idActivo= id
                    };
                    ctx.Entry(act).State = EntityState.Deleted;
                    returno = ctx.SaveChanges();

                    

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

        public IEnumerable<Activo> GetActivo()
        {
            try
            {
                IEnumerable<Activo> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Activo.ToList<Activo>();
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

        public IEnumerable<Activo> GetActivoByDate(string inicio, string final)
        {
            try
            {
                DateTime fechaI = DateTime.Parse(inicio);
                DateTime fechaF = DateTime.Parse(final);
                IEnumerable<Activo> lista = null;

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Activo.ToList<Activo>().FindAll(p=>p.fechaVenceGarantia>=fechaI && p.fechaVenceGarantia<=fechaF);
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

        public Activo GetActivoByID(int id)
        {
            Activo act = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    act = ctx.Activo.Find(id);
                }

                return act;
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


        public IEnumerable<Activo> GetActivoByName(string name)
        {
            IEnumerable<Activo> lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Activo.ToList<Activo>().FindAll(p=> p.descripcion.ToLower().Contains(name.ToLower()));
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

        public bool Save(Activo activo)
        {
            int retorno = 0;
            bool nuevo = false;
            Activo act = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {

                    ctx.Configuration.LazyLoadingEnabled = false;
                    act = GetActivoByID(activo.idActivo);
                    if (act == null)
                    {
                        ctx.Activo.Add(activo);
                        nuevo = true;
                    }
                    else
                    {
                        ctx.Entry(activo).State = EntityState.Modified;
                        nuevo = false;
                    }
                    retorno = ctx.SaveChanges();
                }
                return nuevo;
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

    /*    public void SaveDepreciacion(int id, decimal valor,decimal dolar)
        {
           
            try
            {
                string sql = string.Format("Update Activo set precioColones='%{0}%',precioDolares='%{0}%' where idActivo='%{0}%'",
                      valor, dolar, id);
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    ctx.Activo.SqlQuery(sql);
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
    }
}
