using ApiMETA.Models;
using ApiMETA.Models.Classes;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : ApiController
    {
        /// <summary>
        /// Obtiene un token de autenticación JWT para un usuario.
        /// </summary>
        /// <param name="model">Un objeto AutenticacionLoginModel que contiene las credenciales de usuario.</param>
        /// <returns>
        ///     - IHttpActionResult con un token JWT en caso de éxito.
        ///     - BadRequest con un mensaje de error si el modelo es nulo o falta usuario y contraseña.
        ///     - Unauthorized si la autenticación falla.
        /// </returns>

        [Route("Token")]
        [HttpPost]
        public IHttpActionResult Token(AutenticacionLoginModel model)
        {
            if (model == null)
                return BadRequest("Usuario y Contraseña requeridos.");

            AuthenticationModels objAutenticacion = new AuthenticationModels();
            var _userInfo = objAutenticacion.Login(model);
            if (_userInfo != null)
            {
                if (_userInfo.Usuario != null)
                {
                    // En caso de éxito, genera un token JWT y lo devuelve como respuesta.
                    return Ok(Globals.GenerarTokenJWT(_userInfo));
                }
                else
                {
                    // Si el usuario no es válido, devuelve Unauthorized.
                    return Unauthorized();
                }
            }
            else
            {
                // Si la autenticación falla, devuelve Unauthorized.
                return Unauthorized();
            }
        }
    }
}