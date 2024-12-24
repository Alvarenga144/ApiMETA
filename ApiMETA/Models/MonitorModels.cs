using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class MonitorModels : IDisposable
    {
        public List<MonitorTransactions> SelectMonitorTransactions(MonitorRequest mr)
        {
            List<MonitorTransactions> lst = new List<MonitorTransactions>();
            using (SqlProcess objSql = new SqlProcess())
            {

                DataTable dt = new DataTable();
                dt = objSql.SelectMonitorTransactions(mr.IdUsuario, mr.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new MonitorTransactions()
                    {
                        Id = long.Parse(row["Id"].ToString().Trim()),
                        Fecha = row["Fecha"].ToString().Trim(),
                        Hora = row["Hora"].ToString().Trim(),
                        Terminal = row["TerminalId"].ToString().Trim(),
                        Retailer = row["RetailerId"].ToString().Trim(),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Tarjeta = row["Tarjeta"].ToString().Trim(),
                        Monto = double.Parse(row["Monto"].ToString().Trim()),
                        Autorizacion = row["Autorizacion"].ToString().Trim(),
                        Referencia = row["Referencia"].ToString().Trim(),
                        Adquirente = row["Adquirente"].ToString().Trim(),
                        Tipo = row["Tipo"].ToString().Trim(),
                        Accion = row["Accion"].ToString().Trim(),
                        EVoucher = bool.Parse(row["Voucher"].ToString().Trim()),
                        EDevolucion = bool.Parse(row["Devolucion"].ToString().Trim()) && ((double.Parse(row["Disponible"].ToString().Trim()) > 0)),
                        EAnulacion = bool.Parse(row["Anulacion"].ToString().Trim()) && ((double.Parse(row["Disponible"].ToString().Trim()) == double.Parse(row["Monto"].ToString().Trim()))),
                        Estado = int.Parse(row["Estado"].ToString().Trim()),
                        Respuesta = row["Respuesta"].ToString().Trim(),
                        Adicional = row["Adicional"].ToString().Trim()
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
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~SqlProcess()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class MonitorRequest
    {
        public int IdUsuario { get; set; }
        public string User { get; set; }
    }

    public class MonitorTransactions
    {
        [Display(Name = "ID")]
        public long Id { get; set; }
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }
        [Display(Name = "Hora")]
        public string Hora { get; set; }
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }
        [Display(Name = "Retailer")]
        public string Retailer { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public double Monto { get; set; }
        [Display(Name = "Autorización")]
        public string Autorizacion { get; set; }
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }
        [Display(Name = "Tarjeta")]
        public string Tarjeta { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Adquirente")]
        public string Adquirente { get; set; }
        [Display(Name = "Accion")]
        public string Accion { get; set; }
        [Display(Name = "Adicional")]
        public string Adicional { get; set; }
        [Display(Name = "Respuesta")]
        public string Respuesta { get; set; }
        [Display(Name = "Emitir voucher")]
        public bool EVoucher { get; set; }
        [Display(Name = "Procesar devolución")]
        public bool EDevolucion { get; set; }
        [Display(Name = "Procesar anulación")]
        public bool EAnulacion { get; set; }
        [Display(Name = "Estado")]
        public int Estado { get; set; }
    }
}