using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Infraestructure.Repository
{
   public class RepositoryVendedor : IRepositoryVendedor
    {
        public void DeleteVendedor(string ced)
        {
            int returno;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Vendedor vend = new Vendedor()
                    {
                        cedula = ced
                    };
                    ctx.Entry(vend).State = EntityState.Deleted;
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

        public IEnumerable<Vendedor> GetVendedorByName(string name)
        {
            IEnumerable<Vendedor> lista = null;

            string sql =
                string.Format("select * from Vendedor where nombre like '%{0}%' or apellido like '%{0}%' ", name);
            using (MyContext ctx = new MyContext())
            {
                lista = ctx.Vendedor.SqlQuery(sql).ToList<Vendedor>();
            }

            return lista;
        }

        public IEnumerable<Vendedor> GetVendedor()
        {
            try
            {
                // Forzar error
                // int x = 0;
                // x = 25 / x;


                IEnumerable<Vendedor> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    
                    lista = ctx.Vendedor.ToList<Vendedor>();
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

        public Vendedor GetVendedorByID(string ced)
        {
            Vendedor vend = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    vend = ctx.Vendedor.Find(ced);
                }

                return vend;
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

        public bool Save(Vendedor vendedor)
        {
            int retorno = 0;
            bool nuevo=false;
            Vendedor oVend = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {

                    ctx.Configuration.LazyLoadingEnabled = false;
                    oVend = GetVendedorByID(vendedor.cedula);
                    if (oVend == null)
                    {
                        ctx.Vendedor.Add(vendedor);
                        nuevo = true;
                    }
                    else
                    {
                        ctx.Entry(vendedor).State = EntityState.Modified;
                        nuevo = false;
                    }
                    retorno = ctx.SaveChanges();

                }

              /*  if (retorno >= 0)
                    oVend = GetVendedorByID(vendedor.cedula);*/

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
    }
}
