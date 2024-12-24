using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class StatisticsModels : IDisposable
    {
        public List<StatisticsPurchaseTypeTable> StatisticsPurchaseType(StatisticsRequest mr)
        {
            List<StatisticsPurchaseTypeTable> lst = new List<StatisticsPurchaseTypeTable>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectStatisticsPurchaseType(mr.IdUsuario, mr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    if (double.Parse(row["Monto"].ToString().Trim()) != 0)
                    {
                        lst.Add(new StatisticsPurchaseTypeTable()
                        {
                            Monto = double.Parse(row["Monto"].ToString().Trim()),
                            Cantidad = int.Parse(row["Cantidad"].ToString().Trim()),
                            Accion = row["Accion"].ToString().Trim(),
                            Promedio = double.Parse(row["Monto"].ToString().Trim()) / int.Parse(row["Cantidad"].ToString().Trim())
                        });
                    }
                }
            }
            return lst;
        }

        public List<StatisticsPerCommerceTable> StatisticsPerCommerce(StatisticsRequest mr)
        {
            List<StatisticsPerCommerceTable> lst = new List<StatisticsPerCommerceTable>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectStatisticsCommerce(mr.IdUsuario, mr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new StatisticsPerCommerceTable()
                    {
                        Monto = double.Parse(row["Monto"].ToString().Trim()),
                        Cantidad = int.Parse(row["Cantidad"].ToString().Trim()),
                        Accion = row["Nombre"].ToString().Trim(),
                        Promedio = double.Parse(row["Monto"].ToString().Trim()) / int.Parse(row["Cantidad"].ToString().Trim()),
                        Retailer = row["Retailer"].ToString().Trim()

                    });
                }
            }
            return lst;
        }

        public List<StatisticsPerAcquirerTable> StatisticsPerEachAcquirer(StatisticsRequest mr)
        {
            List<StatisticsPerAcquirerTable> lst = new List<StatisticsPerAcquirerTable>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectStatisticsAcquirer(mr.IdUsuario, mr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new StatisticsPerAcquirerTable()
                    {
                        Monto = double.Parse(row["Monto"].ToString().Trim()),
                        Cantidad = int.Parse(row["Cantidad"].ToString().Trim()),
                        Accion = row["Nombre"].ToString().Trim(),
                        Promedio = double.Parse(row["Monto"].ToString().Trim()) / int.Parse(row["Cantidad"].ToString().Trim())
                    });
                }
            }
            return lst;
        }

        public List<StatisticsPerCommerceTable> StatisticsPerTerminal(StatisticsRequestRetailer mr)
        {
            List<StatisticsPerCommerceTable> lst = new List<StatisticsPerCommerceTable>();
            StatisticsPerCommerceTable obj = new StatisticsPerCommerceTable();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectStatisticsTerminal(mr.IdUsuario, mr.Retailer, mr.User, Globals.IdSistema);
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new StatisticsPerCommerceTable()
                    {
                        Monto = double.Parse(row["Monto"].ToString().Trim()),
                        Cantidad = int.Parse(row["Cantidad"].ToString().Trim()),
                        Accion = row["Terminal"].ToString().Trim(),
                        Promedio = double.Parse(row["Monto"].ToString().Trim()) / int.Parse(row["Cantidad"].ToString().Trim())
                    });
                }
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        obj.Accion = "Total";
                        obj.TotalMonto += lst[i].Monto;
                        obj.TotalCantidad += lst[i].Cantidad;
                    }
                }
            }
            return lst;
        }

        #region IDisposable
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
        // ~StatisticsModels()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class StatisticsRequest
    {
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class StatisticsRequestRetailer
    {
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Retailer")]
        public string Retailer { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class StatisticsPurchaseTypeTable
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Monto { get; set; }

        public int Cantidad { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Promedio { get; set; }

        [Display(Name = "Tipo de Transacción")]
        public string Accion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double TotalMonto { get; set; }

        public int TotalCantidad { get; set; }
    }

    public class StatisticsPerCommerceTable
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Monto { get; set; }

        public int Cantidad { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Promedio { get; set; }

        [DisplayName("Tienda")]
        public string Accion { get; set; }

        [DisplayName("Retailer")]
        public string Retailer { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double TotalMonto { get; set; }

        public int TotalCantidad { get; set; }
    }

    public class StatisticsPerAcquirerTable
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Monto { get; set; }

        public int Cantidad { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Promedio { get; set; }

        [DisplayName("Adquirente")]
        public string Accion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double TotalMonto { get; set; }

        public int TotalCantidad { get; set; }
    }

    public class StatisticsPerTerminal
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Monto { get; set; }

        public int Cantidad { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double Promedio { get; set; }

        [DisplayName("Caja")]
        public string Accion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double TotalMonto { get; set; }

        public int TotalCantidad { get; set; }
    }
}