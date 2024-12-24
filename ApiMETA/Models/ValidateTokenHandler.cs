using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ApiMETA.Models
{
    public class ValidateTokenHandler : DelegatingHandler
    {
        /// <summary>
        /// Procesa una solicitud HTTP y valida un token JWT en la cabecera de autorización.
        /// </summary>
        /// <param name="request">La solicitud HTTP entrante.</param>
        /// <param name="cancellationToken">El token de cancelación para la operación asincrónica.</param>
        /// <returns>Una respuesta HTTP con el resultado de la validación del token.</returns>

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                              CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var claveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
                var issuerToken = ConfigurationManager.AppSettings["Issuer"];
                var audienceToken = ConfigurationManager.AppSettings["Audience"];

                var securityKey = new SymmetricSecurityKey(
                    System.Text.Encoding.Default.GetBytes(claveSecreta));

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // DELEGADO PERSONALIZADO PERA COMPROBAR
                    // LA CADUCIDAD EL TOKEN.
                    LifetimeValidator = this.LifetimeValidator, // Delegado personalizado para comprobar la caducidad del token.
                    IssuerSigningKey = securityKey
                };

                // COMPRUEBA LA VALIDEZ DEL TOKEN
                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token,
                                                                     validationParameters,
                                                                     out SecurityToken securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token,
                                                                      validationParameters,
                                                                      out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException ex)
            {
                string message = ex.Message;
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() =>
                        new HttpResponseMessage(statusCode) { });
        }

        /// <summary>
        /// Intenta recuperar el token de autenticación de la solicitud HTTP.
        /// </summary>
        /// <param name="request">La solicitud HTTP entrante.</param>
        /// <param name="token">El token recuperado, si se encuentra en la cabecera de autorización.</param>
        /// <returns>True si se encuentra y recupera el token, de lo contrario, false.</returns>

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) ||
                                              authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ?
                    bearerToken.Substring(7) : bearerToken;
            return true;
        }

        /// <summary>
        /// Comprueba si el token está dentro de su período de validez (no ha caducado).
        /// </summary>
        /// <param name="notBefore">La fecha en la que el token se vuelve válido.</param>
        /// <param name="expires">La fecha en la que el token caduca.</param>
        /// <param name="securityToken">El token de seguridad.</param>
        /// <param name="validationParameters">Parámetros de validación del token.</param>
        /// <returns>True si el token es válido en cuanto a tiempo, de lo contrario, false.</returns>

        public bool LifetimeValidator(DateTime? notBefore,
                                      DateTime? expires,
                                      SecurityToken securityToken,
                                      TokenValidationParameters validationParameters)
        {
            var valid = false;

            if ((expires.HasValue && DateTime.UtcNow < expires)
                && (notBefore.HasValue && DateTime.UtcNow > notBefore))
            { valid = true; }

            return valid;
        }
    }
}