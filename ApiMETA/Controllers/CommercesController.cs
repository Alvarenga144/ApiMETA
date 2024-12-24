using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Commerces")]
    public class CommercesController : ApiController
    {
        [Authorize]
        [Route("List")]
        [HttpPost]
        public List<ComerciosInfo> ListComercios(ComerciosListRequest comerciosList)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + comerciosList + " \n");
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (CommercesModels objModels = new CommercesModels())
            {
                lst = objModels.SelectComerciosList(comerciosList);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lst) + "  \n");
            return lst;
        }

        [Authorize]
        [Route("Search")]
        [HttpPost]
        public List<ComerciosInfo> SelectList(ComerciosRequest comerciosRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + comerciosRequest + " \n");
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (CommercesModels objModels = new CommercesModels())
            {
                lst = objModels.ComerciosBusqueda(comerciosRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lst) + "  \n");
            return lst;
        }

        [Authorize]
        [Route("Get")]
        [HttpPost]
        public ComerciosInfo GetComercios(ComercioRequestRId comercioRequestRId)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + comercioRequestRId + " \n");
            ComerciosInfo obj = new ComerciosInfo();
            using (CommercesModels objModels = new CommercesModels())
            {
                obj = objModels.GetOneComercio(comercioRequestRId);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(obj) + "  \n");
            return obj;
        }

        [Authorize]
        [Route("Assigned")]
        [HttpPost]
        public List<ComerciosInfo> SelectAsignados(ComerciosListRequest comercioRequestRId)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + comercioRequestRId + " \n");
            List<ComerciosInfo> lst = new List<ComerciosInfo>();
            using (CommercesModels objModels = new CommercesModels())
            {
                lst = objModels.SelectAsignadosLst(comercioRequestRId);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lst) + "  \n");
            return lst;
        }
    }
}