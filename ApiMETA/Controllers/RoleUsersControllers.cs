using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/RoleUsers")]
    public class RoleUsersControllers : ApiController
    {
        [Authorize]
        [Route("AsignRole")]
        [HttpPost]
        public int AdminAsignarl(AdminUserRol adminUserRol)
        {
            int respuesta = 0;
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + adminUserRol + "  \n");
            using (RoleUsersModels objModels = new RoleUsersModels())
            {
                respuesta = objModels.Asignar(adminUserRol);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(respuesta) + "  \n");
            return respuesta;
        }

        [Authorize]
        [Route("RemoveRole")]
        [HttpPost]
        public int AdminRemover(AdminUserRol adminUserRol)
        {
            int respuesta = 0;
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + adminUserRol + "  \n");
            using (RoleUsersModels objModels = new RoleUsersModels())
            {
                respuesta = objModels.Remover(adminUserRol);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(respuesta) + "  \n");
            return respuesta;
        }

        [Authorize]
        [Route("RolesUserList")]
        [HttpPost]
        public List<UsuariosRolesInfo> UserRolList(UserRolRequestList userRolRequestList)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + userRolRequestList + "  \n");
            List<UsuariosRolesInfo> lstUserRol = new List<UsuariosRolesInfo>();
            using (RoleUsersModels objModels = new RoleUsersModels())
            {
                lstUserRol = objModels.UsuarioRolesLst(userRolRequestList);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstUserRol) + "  \n");
            return lstUserRol;
        }

    }
}