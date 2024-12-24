using ApiMETA.Models.Classes;
using ApiMETA.Models.Connections;
using System.Data;

namespace ApiMETA.Models
{
    public class AuthenticationModels
    {
        public AutenticacionInfoModel Login(AutenticacionLoginModel m)
        {
            AutenticacionInfoModel objAutenticacion = new AutenticacionInfoModel();
            if (m != null)
            {
                if (m.Usuario != null && m.Password != null)
                {
                    using (SqlProcess objSql = new SqlProcess())
                    {
                        DataTable dt = new DataTable();
                        dt = objSql.ValidateAutentication(m.Usuario, Globals.Base64Encode(m.Password), m.Codigo);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    objAutenticacion = new AutenticacionInfoModel
                                    {
                                        Id = int.Parse(row["Id"].ToString().Trim()),
                                        Nombre = row["Nombre"].ToString().Trim(),
                                        Apellidos = row["Apellido"].ToString().Trim(),
                                        Usuario = row["Usuario"].ToString().Trim(),
                                    };
                                }
                            }
                        }
                    }
                }
            }
            return objAutenticacion;
        }
    }

    public class AutenticacionInfoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
    }

    public class AutenticacionLoginModel
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public long Codigo { get; set; }
    }

    public class AutenticacionResponseModel
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public double ExpiresIn { get; set; }
    }
}