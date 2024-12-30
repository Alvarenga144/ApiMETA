using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        [Authorize]
        [Route("UsersList")]
        [HttpGet]
        public List<UsuariosInfo> UsuariosList(string Usuario)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: Usuario=" + Usuario + "  \n");
            List<UsuariosInfo> lstUsers = new List<UsuariosInfo>();
            using (UsersModels objModels = new UsersModels())
            {
                lstUsers = objModels.SelectUsuariosList(Usuario);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstUsers) + "  \n");
            return lstUsers;
        }

        [Authorize]
        [Route("UsersSearch")]
        [HttpPost]
        public List<UsuariosInfo> UsuarioBusqueda(UsuariosRequest usuariosRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + usuariosRequest + "  \n");
            List<UsuariosInfo> lstUsers = new List<UsuariosInfo>();
            using (UsersModels objModels = new UsersModels())
            {
                lstUsers = objModels.SelectUsuariosBusqueda(usuariosRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstUsers) + "  \n");
            return lstUsers;
        }

        [Authorize]
        [Route("UserSelected")]
        [HttpPost]
        public UsuariosInfo UsuarioSelected(UsuarioSelected usuarioSelected)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + usuarioSelected + "  \n");
            UsuariosInfo lstUsers = new UsuariosInfo();
            using (UsersModels objModels = new UsersModels())
            {
                lstUsers = objModels.UsuarioSeleccionado(usuarioSelected);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstUsers) + "  \n");
            return lstUsers;
        }

        [Authorize]
        [Route("AdminUsers")]
        [HttpPost]
        public int AdminUsuarios(AdminUsuarios admin)
        {
            int respuesta = 0;
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + admin + " \n");
            using (UsersModels objModels = new UsersModels())
            {
                respuesta = objModels.AdminUsuario(admin);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(respuesta) + "  \n");
            return respuesta;
        }
    }
}