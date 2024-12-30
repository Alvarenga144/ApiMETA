using ApiMETA.Models.Classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace ApiMETA.Models.Connections
{
    public class SqlProcess : IDisposable
    {
        #region Vars

        /*
         * CONEXIONES A LOS DIFERENTES SERVIDORES CON LAS DB NECESARIAS
         */

        private SqlConnection _ExSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EConnectionString"].ConnectionString);
        private SqlConnection _CtSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CConnectionString"].ConnectionString);
        private SqlConnection _HtSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HConnectionString"].ConnectionString);
        private SqlConnection _TgSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TConnectionString"].ConnectionString);
        private SqlConnection _HPSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DConnectionString"].ConnectionString);
        private SqlConnection _DbSQLConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DConnectionString"].ConnectionString);
        private long _IdSistema = long.Parse(System.Configuration.ConfigurationManager.AppSettings["IdSystem"]);
        private string _phrase = System.Configuration.ConfigurationManager.AppSettings["PhraseMail"];
        private string _NombreSistema = System.Configuration.ConfigurationManager.AppSettings["NameSystem"];

        public SqlProcess()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        #endregion

        #region Monitor

        public DataTable SelectMonitorTransactions(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Monitor_Transacciones_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectMonitorPurchaseType(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Monitor_Tipo_Compra_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectMonitorApproved(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Monitor_Aprobacion_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region Transactions

        public DataTable SelectTransaciones(DateTime Desde, DateTime Hasta, string Retailer, string Nombre, string Terminal, string Autorizacion, string Tipo, double Monto, int IdUSuario, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Transacciones_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Desde", Desde).SqlDbType = SqlDbType.Date;
                    da.SelectCommand.Parameters.AddWithValue("@Hasta", Hasta).SqlDbType = SqlDbType.Date;
                    da.SelectCommand.Parameters.AddWithValue("@Retailer", Retailer).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Nombre", Nombre).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Terminal", Terminal).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Autorizacion", Autorizacion).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Tipo", Tipo).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Monto", Monto).SqlDbType = SqlDbType.Decimal;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUSuario).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable GetTransaciones(long Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Transacciones_Get", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.BigInt;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataSet InsertTransacionHistorial(long IdTxn, DateTime FechaOrig, string HoraOrig, string TerminalId, string RetailerId,
            string Nombre, string PrimaryIin, string PrimaryPanTail, double MontoOrig, double MontoAdicionalOrig, string AutorizacionOrig,
            string ReferenciaOrig, string ReciboOrig, string DataAdicional, string ProcessingCode, string Mti, string Autorizacion,
            string Recibo, double Monto, double MontoAdicional, string Referencia, string Fecha, string Hora, int Tipo, int IdUsuario, string User, long IdSistema)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Transacciones_Insert", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdTxn", IdTxn).SqlDbType = SqlDbType.BigInt;
                    da.SelectCommand.Parameters.AddWithValue("@FechaOrig", FechaOrig).SqlDbType = SqlDbType.Date;
                    da.SelectCommand.Parameters.AddWithValue("@HoraOrig", HoraOrig).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@TerminalId", TerminalId).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@RetailerId", RetailerId).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Nombre", Nombre).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@PrimaryIin", PrimaryIin).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@PrimaryPanTail", PrimaryPanTail).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@MontoOrig", MontoOrig).SqlDbType = SqlDbType.Money;
                    da.SelectCommand.Parameters.AddWithValue("@MontoAdicionalOrig", MontoAdicionalOrig).SqlDbType = SqlDbType.Money;
                    da.SelectCommand.Parameters.AddWithValue("@AutorizacionOrig", AutorizacionOrig).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@ReferenciaOrig", ReferenciaOrig).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@ReciboOrig", ReciboOrig).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@DataAdicional", DataAdicional).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@ProcessingCode", ProcessingCode).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Mti", Mti).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Autorizacion", Autorizacion).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Recibo", Recibo).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Monto", Monto).SqlDbType = SqlDbType.Money;
                    da.SelectCommand.Parameters.AddWithValue("@MontoAdicional", MontoAdicional).SqlDbType = SqlDbType.Money;
                    da.SelectCommand.Parameters.AddWithValue("@Referencia", Referencia).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Fecha", Fecha).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Hora", Hora).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Tipo", Tipo).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@User", User).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return ds;
        }

        public DataTable SelectTipoTransaciones(string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Tipo_Transacciones_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region Statistics

        public DataTable SelectStatisticsPurchaseType(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Estadisticas_Tipo_Compra_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");

                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectStatisticsCommerce(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Estadisticas_Por_Sucursal_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectStatisticsAcquirer(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Estadisticas_Por_Adquirente_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectStatisticsTerminal(int Id, string Retailer, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Estadisticas_Por_Terminal_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Id).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Retailer", Retailer).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region Commerces

        public DataTable SelectCommerces(int IdUsuario, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Comercios_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable CommerceSearch(int IdUsuario, string Descripcion, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Comercios_Search", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Descripcion", Descripcion).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable GetCommerces(int IdUsuario, long Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Comercios_Get", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.BigInt;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region Users and role's users

        public DataTable SelectUsuarios(string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable LikeUsuarios(string Descripcion, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Like", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Descripcion", Descripcion).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable GetUsuarioSelected(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Get", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable AdminUsuarios(int Tipo, int Id, string Nombre, string Apellido, string Usuario, string Clave, string Email, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Admin", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Tipo", Tipo).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Nombre", Nombre).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Apellido", Apellido).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Usuario", Usuario).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Clave", Clave).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Email", Email).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@User", User).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable ValidarUsuarios(string Usuario, string Clave, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Validate", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Usuario", Usuario).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Clave", Clave).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, Usuario, IdSistema);
            }
            return dt;
        }

        public DataTable FindUsuarios(string Usuario, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Find", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Usuario", Usuario).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, Usuario, IdSistema);
            }
            return dt;
        }

        public DataTable AdminUsuariosRoles(int Tipo, int IdUsuario, int IdRol, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Roles_Admin", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Tipo", Tipo).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@IdRol", IdRol).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@User", User).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable GetUsuariosRoles(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Roles_Get", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable SelectUsuariosRoles(int Id, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Roles_Select", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable LikeUsuariosRoles(int Id, string Descripcion, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Roles_Like", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Id", Id).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Descripcion", Descripcion).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region Asignations

        // 1-ASIGNATION O 2-REMOVE
        public DataTable AdminUsuarioComercio(int Tipo, int IdUSuario, long IdSucursal, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Comercios_Admin", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@Tipo", Tipo).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUSuario).SqlDbType = SqlDbType.Int;
                    da.SelectCommand.Parameters.AddWithValue("@Idcomercio", IdSucursal).SqlDbType = SqlDbType.BigInt;
                    da.SelectCommand.Parameters.AddWithValue("@User", User).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        public DataTable GetUsuarioComercio(int IdUsuario, string User, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("META.Sp_Usuarios_Comercios_Get", _ExSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 5000;
                    da.SelectCommand.Parameters.AddWithValue("@IdUSuario", IdUsuario).SqlDbType = SqlDbType.Int;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, User, IdSistema);
            }
            return dt;
        }

        #endregion

        #region JWT

        public DataTable ValidateAutentication(string Usuario, string Clave, long IdSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Seguridad.Sp_Autenticacion_Validate", _CtSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Usuario", Usuario).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Clave", Clave).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@IdSistema", IdSistema).SqlDbType = SqlDbType.BigInt;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception e)
            {
                SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, Usuario, Globals.IdSistema);
            }
            return dt;
        }

        #endregion

        #region Exceptions
        public void SaveExceptions(string Funcion_Name, string Exception, string User, long IdSistema)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Sistemas.Sp_Log", _CtSQLConnection))
                {
                    string ipAddress = "";
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.ToString();
                        }
                    }
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Equipo", Dns.GetHostName().ToString()).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@IpAddress", ipAddress).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Funcion", Funcion_Name).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@Exeption", Exception).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@User", User).SqlDbType = SqlDbType.VarChar;
                    da.SelectCommand.Parameters.AddWithValue("@IdSistema", IdSistema).SqlDbType = SqlDbType.BigInt;
                    da.Fill(ds);
                    /*
                    try
                    {

                        string asunto = "Alerta de Errores en Aplicaciones Institucionales";
                        string datos = "";
                        datos = "<h3>Detalles:</h3> " +
                                "<p><b>Sistema: </b>" + _NombreSistema +
                                "<p><b>Usuario: </b>" + User +
                                "<p><b>Funci&oacute;n: </b>" + Funcion_Name +
                                "<p><b>Excepci&oacute;n: </b>" + Exception +
                                "</br>" +
                                "<h5><font color=\"red\">Por favor no responda este correo electr&oacute;nico, este es generado por un sistema. </font><h5>";
                        using (EmailSender objEmail = new EmailSender())
                        {
                            bool resp = objEmail.SendMail(GetParamsDt(_IdSistema), GetRemitenteError(), asunto, datos, User);
                        }
                    }
                    catch (Exception) { }
                    */
                }
            }
            catch (Exception e)
            {
                _ = e.Message;
            }
        }

        public DataTable GetParamsDt(long idSistema)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Sistemas.Sp_Parameters_Get", _CtSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@IdSistema", idSistema).SqlDbType = SqlDbType.BigInt;
                    da.SelectCommand.Parameters.AddWithValue("@Phrase", _phrase).SqlDbType = SqlDbType.VarChar;
                    da.Fill(ds, "Datos");
                    dt = ds.Tables["Datos"];
                }
            }
            catch (Exception)
            {

            }
            return dt;
        }

        public DataTable GetRemitenteError()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Mensajeria.Sp_Errors_Destinations_Get", _CtSQLConnection))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {

            }
            return dt;
        }

        #endregion

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
}