using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiMETA.Models
{
    public class UsersModels : IDisposable
    {
        private string _PhraseCripto = System.Configuration.ConfigurationManager.AppSettings["CryptoKey"];

        public List<UsuariosInfo> SelectUsuariosList(string Usuario)
        {
            List<UsuariosInfo> lst = new List<UsuariosInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.SelectUsuarios(Usuario, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new UsuariosInfo()
                    {
                        Id = int.Parse(row["Id"].ToString().Trim()),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Apellido = row["Apellido"].ToString().Trim(),
                        Usuario = row["Usuario"].ToString().Trim(),
                        Clave = "***",
                        Email = row["Email"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        public List<UsuariosInfo> SelectUsuariosBusqueda(UsuariosRequest us)
        {
            List<UsuariosInfo> lst = new List<UsuariosInfo>();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.LikeUsuarios(us.Descripcion, us.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new UsuariosInfo()
                    {
                        Id = int.Parse(row["Id"].ToString().Trim()),
                        Nombre = row["Nombre"].ToString().Trim(),
                        Apellido = row["Apellido"].ToString().Trim(),
                        Usuario = row["Usuario"].ToString().Trim(),
                        Clave = "***",
                        Email = row["Email"].ToString().Trim()
                    });
                }
            }
            return lst;
        }

        public UsuariosInfo UsuarioSeleccionado(UsuarioSelected users)
        {
            UsuariosInfo obj = new UsuariosInfo();
            using (SqlProcess objSql = new SqlProcess())
            {
                DataTable dt = new DataTable();
                dt = objSql.GetUsuarioSelected(users.IdUsuario, users.User, Globals.IdSistema);

                foreach (DataRow row in dt.Rows)
                {
                    obj.Id = int.Parse(row["Id"].ToString().Trim());
                    obj.Nombre = row["Nombre"].ToString().Trim();
                    obj.Apellido = row["Apellido"].ToString().Trim();
                    obj.Usuario = row["Usuario"].ToString().Trim();
                    obj.Clave = "***";
                    obj.Email = row["Email"].ToString().Trim();
                }
            }
            return obj;
        }

        public int AdminUsuario(AdminUsuarios admin)
        {
            int Respuesta = 0;
            using (SqlProcess objSql = new SqlProcess())
            {
                if (admin.Tipo == 1 || admin.Tipo == 5)
                {
                    string claveGenerada = Globals.Generate(admin.Usuario, Globals.IdSistema);
                    string claveEncrypt = "";

                    DataTable dt = new DataTable();
                    using (Crypto objCrypto = new Crypto(_PhraseCripto))
                    {
                        claveEncrypt = objCrypto.EncryptFromBase64(claveGenerada);
                    }
                    dt = objSql.AdminUsuarios(admin.Tipo, admin.IdUsuario, admin.Nombre, admin.Apellido, admin.Usuario, claveEncrypt, admin.Email, admin.User, Globals.IdSistema);
                    foreach (DataRow row in dt.Rows)
                    {
                        Respuesta = int.Parse(row["Respuesta"].ToString().Trim());
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    using (Crypto objCrypto = new Crypto(_PhraseCripto))
                    {
                        dt = objSql.AdminUsuarios(admin.Tipo, admin.IdUsuario, admin.Nombre, admin.Apellido, admin.Usuario, objCrypto.EncryptFromBase64(admin.Clave), admin.Email, admin.User, Globals.IdSistema);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        Respuesta = int.Parse(row["Respuesta"].ToString().Trim());
                    }
                }
            }
            return Respuesta;
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
        // ~UsersModels()
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

    public class AdminUsuarios
    {
        [Range(1, long.MaxValue, ErrorMessage = "Campo Tipo debe ser mayor que cero")]
        [Display(Name = "Tipo")]
        public int Tipo { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Campo Id Usuario debe ser mayor que cero")]
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo nombre debe tener hasta 25 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo Apellido debe tener hasta 25 caracteres")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo usuario debe tener hasta 25 caracteres")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo user debe tener hasta 25 caracteres")]
        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class UsuariosRequest
    {
        [StringLength(15, MinimumLength = 0, ErrorMessage = "Campo descripcion debe tener hasta 15 caracteres")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo usuario debe tener hasta 25 caracteres")]
        [Display(Name = "Usuario")]
        public string User { get; set; }
    }

    public class UsuarioSelected
    {
        [Range(1, long.MaxValue, ErrorMessage = "Campo Id Usuario debe ser mayor que cero")]
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [StringLength(25, MinimumLength = 0, ErrorMessage = "Campo usuario debe tener hasta 25 caracteres")]
        [Display(Name = "Usuario")]
        public string User { get; set; }
    }

    public class UsuariosInfo
    {
        [DisplayName("Id Usuario")]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "50 Caracteres máximo")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "50 Caracteres máximo")]
        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [StringLength(25, ErrorMessage = "25 Caracteres máximo")]
        [DisplayName("Usuario")]
        public string Usuario { get; set; }

        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "25 Caracteres máximo")]
        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "25 Caracteres máximo")]
        [DisplayName("Confirmar Clave")]
        public string ReClave { get; set; }

        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "25 Caracteres máximo")]
        [DisplayName("Nueva Clave")]
        public string NewClave { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(75, ErrorMessage = "75 Caracteres máximo")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Cambio")]
        public bool Cambio { get; set; }

        [Display(Name = "Fecha de cambio")]
        public DateTime FechaCambio { get; set; }
    }
}