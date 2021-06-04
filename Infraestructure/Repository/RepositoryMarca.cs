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
    public class RepositoryMarca : IRepositoryMarca
    {
        public void DeleteMarca(int id)
        {
            int returno;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Marca mar = new Marca()
                    {
                        idMarca = id
                    };
                    ctx.Entry(mar).State = EntityState.Deleted;
                    returno = ctx.SaveChanges();
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

        public IEnumerable<Marca> GetMarca()
        {
            try
            {
                IEnumerable<Marca> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Marca.ToList<Marca>();
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

        public Marca GetMarcaByID(int id)
        {
            Marca mar = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    mar = ctx.Marca.Find(id);
                }

                return mar;
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

        public bool Save(Marca marca)
        {
            int retorno = 0;
            bool nuevo = false;
            Marca mar = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {

                    ctx.Configuration.LazyLoadingEnabled = false;
                    mar = GetMarcaByID(marca.idMarca);
                    if (mar == null)
                    {
                        ctx.Marca.Add(marca);
                        nuevo = true;
                    }
                    else
                    {
                        ctx.Entry(marca).State = EntityState.Modified;
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
       
        public IEnumerable<Marca> GetMarcaByName(string name)
        {
            IEnumerable<Marca> lista = null;

            string sql =
                string.Format("select * from Marca where descripcion like '%{0}%' ", name);
            using (MyContext ctx = new MyContext())
            {
                lista = ctx.Marca.SqlQuery(sql).ToList<Marca>();
            }

            return lista;
        }
    }
}
