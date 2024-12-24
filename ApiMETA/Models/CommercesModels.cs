using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class CommercesModels : IDisposable
    {
        public List<ComerciosInfo> SelectComerciosList(ComerciosListRequest cr)
        {
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectCommerces(cr.IdUsuario, cr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new ComerciosInfo()
                    {
                        Id = long.Parse(row["Id"].ToString().Trim()),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Retailer = row["Retailer"].ToString().Trim(),
                        Direccion = row["Direccion"].ToString().Trim(),
                        Fecha = row["Fecha"].ToString().Trim(),
                        Adquirente = row["Adquirente"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        public List<ComerciosInfo> ComerciosBusqueda(ComerciosRequest cr)
        {
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.CommerceSearch(cr.IdUsuario, cr.Descripcion, cr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new ComerciosInfo()
                    {
                        Id = long.Parse(row["Id"].ToString().Trim()),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Retailer = row["Retailer"].ToString().Trim(),
                        Direccion = row["Direccion"].ToString().Trim(),
                        Fecha = row["Fecha"].ToString().Trim(),
                        Adquirente = row["Adquirente"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        public ComerciosInfo GetOneComercio(ComercioRequestRId mrrid)
        {
            ComerciosInfo obj = new ComerciosInfo();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.GetCommerces(mrrid.IdUsuario, mrrid.IdRetailer, mrrid.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    obj.Id = long.Parse(row["Id"].ToString().Trim());
                    obj.Nombre = row["Nombre"].ToString().Trim();
                    obj.Retailer = row["Retailer"].ToString().Trim();
                    obj.Direccion = row["Direccion"].ToString().Trim();
                    obj.Fecha = row["Fecha"].ToString().Trim();
                    obj.Adquirente = row["Adquirente"].ToString().Trim();
                }
            }
            return obj;
        }

        public List<ComerciosInfo> SelectAsignadosLst(ComerciosListRequest clr)
        {
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.GetUsuarioComercio(clr.IdUsuario, clr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new ComerciosInfo()
                    {
                        Id = long.Parse(row["Id"].ToString().Trim()),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Retailer = row["Retailer"].ToString().Trim(),
                        Direccion = row["Direccion"].ToString().Trim(),
                        Fecha = row["Fecha"].ToString().Trim(),
                        Adquirente = row["Adquirente"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        #region Disposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ComerciosModels()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class ComerciosRequest
    {
        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class ComerciosListRequest
    {
        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class ComercioRequestRId
    {
        public int IdUsuario { get; set; }
        public long IdRetailer { get; set; }
        public string User { get; set; }
    }

    public class ComerciosInfo
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Display(Name = "Retailer")]
        public string Retailer { get; set; }

        [Display(Name = "Comercio")]
        public string Nombre { get; set; }

        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Display(Name = "Adquiriente")]
        public string Adquirente { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
    }
}