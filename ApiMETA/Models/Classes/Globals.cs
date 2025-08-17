using ApiMETA.Models.Connections;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace ApiMETA.Models.Classes
{
    public class Globals
    {
        #region Vars

        public static string Ruta = ConfigurationManager.AppSettings["Ruta"];
        public static int TimeOut = int.Parse(ConfigurationManager.AppSettings["TimeOut"]);
        public static long IdSistema = long.Parse(ConfigurationManager.AppSettings["IdSystem"]);
        public static string phrase = ConfigurationManager.AppSettings["PhraseMail"];
        public static string NombreSistema = ConfigurationManager.AppSettings["NameSystem"];
        public static int Usuario = int.Parse(ConfigurationManager.AppSettings["Usuario"]);
        public static string PathLog = ConfigurationManager.AppSettings["PathLog"];
        public static string strPDFFileName = "Voucher";
        public static string PDFPath = "E:\\Vouchers";

        #endregion

        #region Log

        public static bool Log(string text)
        {
            const int maxAttempts = 3;
            int count = 1;
            while (count <= maxAttempts)
            {
                try
                {
                    string FileName = NombreSistema + count.ToString() + "-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
                    StringBuilder sb = new StringBuilder();
                    sb.Append(text);

                    // flush every 20 seconds as you do it
                    File.AppendAllText(PathLog + FileName, sb.ToString());
                    sb.Clear();
                    return true;
                }
                catch (Exception ex)
                {
                    if (count >= maxAttempts)
                    {
                        try
                        {
                            using (SqlProcess objsql = new SqlProcess())
                            {
                                objsql.SaveExceptions(nameof(Log), ex.Message, Usuario.ToString(), IdSistema);
                            }
                        }
                        catch
                        {
                            // Intentionally ignore to avoid throwing from log method
                        }
                        return false;
                    }
                    count++;
                }
            }
            return false;
        }

        #endregion

        #region password

        public static char[] special = ("!@#$%&?!@#$%&*?").ToCharArray();
        public static string GeneratePassword(string user)
        {
            string pass = "";

            try
            {
                Random rnd = new Random();
                char randomChar = (char)rnd.Next('a', 'z');
                int dice = rnd.Next(1, 10);

                pass = randomChar.ToString().ToUpper() + System.Web.Security.Membership.GeneratePassword(8, 0) + randomChar.ToString() + dice.ToString() + special[dice].ToString();
            }
            catch (Exception e)
            {
                using (SqlProcess objsql = new SqlProcess())
                {
                    objsql.SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, user, IdSistema);
                }
            }
            return pass;
        }

        public static string Generate(string User, long IdSistema)
        {
            string pass = GeneratePassword(User);
            return pass;
        }

        #endregion

        #region Base64

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #endregion

        #region JWT

        public static AutenticacionResponseModel GenerarTokenJWT(AutenticacionInfoModel usuarioInfo)
        {
            AutenticacionResponseModel objAut = new AutenticacionResponseModel();
            // RECUPERAMOS LAS VARIABLES DE CONFIGURACIÓN
            var _ClaveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
            var _Issuer = ConfigurationManager.AppSettings["Issuer"];
            var _Audience = ConfigurationManager.AppSettings["Audience"];
            var _Expires = Int32.Parse(ConfigurationManager.AppSettings["Expires"]);
            var _Type = ConfigurationManager.AppSettings["Type"];

            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_ClaveSecreta));
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()),
                new Claim("nombre", usuarioInfo.Nombre),
                new Claim("apellido", usuarioInfo.Apellidos),
                new Claim("usuario", usuarioInfo.Usuario),
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _Issuer,
                    audience: _Audience,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra en X minutos.
                    expires: DateTime.UtcNow.AddMinutes(_Expires)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );
            objAut.Token = new JwtSecurityTokenHandler().WriteToken(_Token);
            objAut.TokenType = _Type;
            objAut.ExpiresIn = (DateTime.UtcNow.AddMinutes(_Expires) - DateTime.UtcNow).TotalMilliseconds;
            return objAut;
        }

        #endregion
    }
}