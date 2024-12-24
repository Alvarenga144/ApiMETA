using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Monitor")]
    public class MonitorController : ApiController
    {
        [Authorize]
        [Route("Select")]
        [HttpPost]
        public List<MonitorTransactions> monitorTransaccionesSelect(MonitorRequest monitorRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + monitorRequest + "  \n");
            List<MonitorTransactions> lstTransacciones = new List<MonitorTransactions>();
            using (MonitorModels objModels = new MonitorModels())
            {
                lstTransacciones = objModels.SelectMonitorTransactions(monitorRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstTransacciones) + "  \n");
            return lstTransacciones;
        }
    }
}