using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class RoleUsersModels : IDisposable
    {
        public int Asignar(AdminUserRol admin)
        {
            int Respuesta = 0;
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.AdminUsuariosRoles(1, admin.IdUsuario, admin.IdRol, admin.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    Respuesta = int.Parse(row["Respuesta"].ToString().Trim());
                }
            }
            return Respuesta;
        }

        public int Remover(AdminUserRol admin)
        {
            int Respuesta = 0;
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.AdminUsuariosRoles(2, admin.IdUsuario, admin.IdRol, admin.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    Respuesta = int.Parse(row["Respuesta"].ToString().Trim());
                }
            }
            return Respuesta;
        }

        public List<UsuariosRolesInfo> UsuarioRolesLst(UserRolRequestList urrl)
        {
            List<UsuariosRolesInfo> lst = new List<UsuariosRolesInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.GetUsuariosRoles(urrl.IdUsuario, urrl.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new UsuariosRolesInfo()
                    {
                        IdRol = int.Parse(row["IdRol"].ToString().Trim()),
                        IdUsuario = int.Parse(row["IdUsuario"].ToString().Trim()),
                        Rol = row["Rol"].ToString().Trim(),
                        Usuario = row["Usuario"].ToString().Trim(),
                        Fecha = row["Fecha"].ToString().Trim(),
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

    public class AdminUserRol
    {
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Id Rol")]
        public int IdRol { get; set; }

        [Display(Name = "Usuario")]
        public string User { get; set; }
    }

    public class UserRolRequestList
    {
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Usuario")]
        public string User { get; set; }
    }

    public class UsuariosRolesInfo
    {
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Display(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "IdRol")]
        public int IdRol { get; set; }

        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
    }
}