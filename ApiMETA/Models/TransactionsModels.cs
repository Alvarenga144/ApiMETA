using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class TransactionsModels : IDisposable
    {
        #region Transactions

        public List<TipoTransaccion> SelectTipoTransaccion(string Usuario)
        {
            List<TipoTransaccion> lst = new List<TipoTransaccion>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectTipoTransaciones(Usuario, Globals.IdSistema);
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new TipoTransaccion { Id = row["Tipo"].ToString(), Tipo = row["Tipo"].ToString() });
                }
            }
            return lst;
        }

        public List<TransaccionInfo> SelectTransacciones(TransaccionRequest tr)
        {
            List<TransaccionInfo> lst = new List<TransaccionInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectTransaciones(tr.Desde, tr.Hasta, tr.Retailer, tr.Nombre, tr.Terminal, tr.Autorizacion, tr.Tipo, tr.Monto, tr.IdUsuario, tr.Usuario, Globals.IdSistema);
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new TransaccionInfo()
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
                        Abreviatura = row["Abreviatura"].ToString().Trim(),
                        Tipo = row["Tipo"].ToString().Trim(),
                        Accion = row["Accion"].ToString().Trim(),
                        EVoucher = bool.Parse(row["Voucher"].ToString().Trim()),
                        EDevolucion = (bool.Parse(row["Devolucion"].ToString().Trim())) ? ((double.Parse(row["Disponible"].ToString().Trim()) > 0) ? true : false) : false,
                        EAnulacion = (bool.Parse(row["Anulacion"].ToString().Trim())) ? ((double.Parse(row["Disponible"].ToString().Trim()) == double.Parse(row["Monto"].ToString().Trim())) ? true : false) : false,
                        Disponible = double.Parse(row["Disponible"].ToString().Trim()),
                        Estado = int.Parse(row["Estado"].ToString().Trim()),
                        Respuesta = row["Respuesta"].ToString().Trim(),
                        Adicional = row["Adicional2"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        public TransaccionInfo GetTransaccion(TransaccionGet tg)
        {
            TransaccionInfo obj = new TransaccionInfo();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.GetTransaciones(tg.IdTransaccion, tg.Usuario, Globals.IdSistema);
                foreach (DataRow row in dt.Rows)
                {
                    obj.Id = long.Parse(row["Id"].ToString().Trim());
                    obj.Fecha = row["Fecha"].ToString().Trim();
                    obj.Hora = row["Hora"].ToString().Trim();
                    obj.Terminal = row["TerminalId"].ToString().Trim();
                    obj.Retailer = row["RetailerId"].ToString().Trim();
                    obj.Nombre = row["Nombre"].ToString().Trim();
                    obj.Tarjeta = row["Tarjeta"].ToString().Trim();
                    obj.Monto = double.Parse(row["Monto"].ToString().Trim());
                    obj.Autorizacion = row["Autorizacion"].ToString().Trim();
                    obj.Referencia = row["Referencia"].ToString().Trim();
                    obj.Tipo = row["Tipo"].ToString().Trim();
                    obj.Adquirente = row["Adquirente"].ToString().Trim();
                    obj.Abreviatura = row["Abreviatura"].ToString().Trim();
                    obj.Accion = row["Accion"].ToString().Trim();
                    obj.Meses = int.Parse(row["Meses"].ToString().Trim());
                    obj.Cuota = (obj.Meses > 0) ? obj.Monto / obj.Meses : 0;
                    obj.Disponible = double.Parse(row["Disponible"].ToString().Trim());
                    obj.Adicional = row["Adicional"].ToString().Trim();
                }
            }
            return obj;
        }

        #endregion

        public bool SendVoucher(VoucherRequest vRequest)
        {
            bool respuesta = true;// = Globals.SendMailVoucher(vRequest.IdTransaccion, vRequest.Email, vRequest.Nombre, vRequest.User, Globales.IdSistema);
            return respuesta;
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
        // ~TransaccionModels()
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

    public class AdminTxnRequest
    {
        [Required(ErrorMessage = "Terminal es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Terminal debe tener 8 caracteres")]
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }

        [Required(ErrorMessage = "Retailer es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Retailer debe tener 15 caracteres")]
        [Display(Name = "Retailer")]
        public string Retailer { get; set; }

        //[Required(ErrorMessage = "Autorización es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(6, MinimumLength = 0, ErrorMessage = "Autorización debe tener 6 caracteres")]
        [Display(Name = "Autorización")]
        public string Autorizacion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(12, MinimumLength = 0, ErrorMessage = "Referencia debe tener un maximo de 12 caracteres")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue, ErrorMessage = "Monto debe ser un valor numerico")]
        [Display(Name = "Monto")]
        public double Monto { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Adicionales debe tener un maximo de 100 caracteres")]
        public string Adicionales { get; set; }

        [Required(ErrorMessage = "Fecha es un campo obligatorio y con Tipo fecha")]
        [DataType(DataType.Date, ErrorMessage = "Fecha debe tener formato Fecha y Hora")]
        [Display(Name = "Fecha")]
        public DateTime? Fecha { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Campo Id Usuario debe ser mayor que cero")]
        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Usuario es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Usuario debe tener de 3 a 25 caracteres")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
    }

    public class TransaccionRequest
    {
        [Required(ErrorMessage = "Campo fecha desde es obligatorio")]
        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        public DateTime Desde { get; set; }

        [Required(ErrorMessage = "Campo fecha hasta es obligatorio")]
        [Display(Name = "Hasta")]
        [DataType(DataType.Date)]
        public DateTime Hasta { get; set; }

        [StringLength(15, MinimumLength = 0, ErrorMessage = "Campo retailer debe tener hasta 15 caracteres")]
        [Display(Name = "Retailer")]
        public string Retailer { get; set; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Campo retailer debe tener hasta 50 caracteres")]
        [Display(Name = "Nombre Sucursal")]
        public string Nombre { get; set; }

        [StringLength(8, MinimumLength = 0, ErrorMessage = "Campo terminal debe tener hasta 8 caracteres")]
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Monto debe ser un valor numerico")]
        [Display(Name = "Monto")]
        public double Monto { get; set; }

        [StringLength(6, MinimumLength = 0, ErrorMessage = "Campo autorización debe tener hasta 6 caracteres")]
        [Display(Name = "Autorización")]
        public string Autorizacion { get; set; }

        [StringLength(7, MinimumLength = 0, ErrorMessage = "Campo tipo debe tener hasta 7 caracteres")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Campo Id Usuario debe ser mayor que cero")]
        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo usuario debe tener hasta 25 caracteres")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
    }

    public class VoucherRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Campo Id Transaccion debe ser mayor que cero")]
        [Display(Name = "Id Transaccion")]
        public long IdTransaccion { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo user debe tener hasta 25 caracteres")]
        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class TransaccionGet
    {
        [Range(1, long.MaxValue, ErrorMessage = "Campo Id Transaccion debe ser mayor que cero")]
        [Display(Name = "Id Transaccion")]
        public long IdTransaccion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Campo Id Usuario debe ser mayor que cero")]
        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo usuario debe tener hasta 25 caracteres")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
    }

    public class TransaccionInfo
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
        [Display(Name = "Recibo")]
        public string Recibo { get; set; }
        [Display(Name = "Emisor")]
        public string Emisor { get; set; }
        [Display(Name = "Tarjeta")]
        public string Tarjeta { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Tipo2")]
        public string Tipo2 { get; set; }
        [Display(Name = "Adquirente")]
        public string Adquirente { get; set; }
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Display(Name = "Abreviatura")]
        public string Abreviatura { get; set; }
        [Display(Name = "Accion")]
        public string Accion { get; set; }
        [Display(Name = "Meses")]
        public int Meses { get; set; }
        [Display(Name = "Cuota")]
        public double Cuota { get; set; }
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
        [Display(Name = "Devolucion")]
        public double Devolucion { get; set; }
        [Display(Name = "Disponible")]
        public double Disponible { get; set; }
        [Display(Name = "Estado")]
        public int Estado { get; set; }
        [Display(Name = "Trans")]
        public string Trans { get; set; }
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }
    }

    public class TipoTransaccion
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
    }

    public class TransaccionRespuesta
    {
        [Required(ErrorMessage = "Terminal es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Terminal debe tener 8 caracteres")]
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }

        [Required(ErrorMessage = "Retailer es un campo obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Retailer debe tener 15 caracteres")]
        [Display(Name = "Retailer")]
        public string Retailer { get; set; }

        [DataType(DataType.Text)]
        [StringLength(6, MinimumLength = 0, ErrorMessage = "Autorización debe tener 6 caracteres")]
        [Display(Name = "Autorización")]
        public string Autorizacion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(3, MinimumLength = 0, ErrorMessage = "Codigo de Respuesta debe tener hasta 3 caracteres")]
        [Display(Name = "Codigo de Respuesta")]
        public string CodigoRespuesta { get; set; }

        [DataType(DataType.Text)]
        [StringLength(12, MinimumLength = 0, ErrorMessage = "Referencia debe tener un maximo de 12 caracteres")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Numero de auditoria debe tener un maximo de 100 caracteres")]
        public string NoAuditoria { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Monto debe ser un valor numerico")]
        [Display(Name = "Referencia")]
        public double Monto { get; set; }

        [Required(ErrorMessage = "Fecha es un campo obligatorio")]
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Required(ErrorMessage = "Hora es un campo obligatorio")]
        [Display(Name = "Hora")]
        public string Hora { get; set; }
    }
}