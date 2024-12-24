using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Statistics")]

    public class EstadisticasController : ApiController
    {
        [Authorize]
        [Route("PurchasesTable")]
        [HttpPost]
        public List<StatisticsPurchaseTypeTable> EstadTipoCompraTbl(StatisticsRequest EstRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + EstRequest + " \n");
            List<StatisticsPurchaseTypeTable> lstGrafico = new List<StatisticsPurchaseTypeTable>();
            using (StatisticsModels objModels = new StatisticsModels())
            {
                lstGrafico = objModels.StatisticsPurchaseType(EstRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstGrafico) + "  \n");
            return lstGrafico;
        }

        [Authorize]
        [Route("ShopTable")]
        [HttpPost]
        public List<StatisticsPerCommerceTable> EstadPorTiendaTbl(StatisticsRequest EstRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + EstRequest + " \n");
            List<StatisticsPerCommerceTable> lstGrafico = new List<StatisticsPerCommerceTable>();
            using (StatisticsModels objModels = new StatisticsModels())
            {
                lstGrafico = objModels.StatisticsPerCommerce(EstRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstGrafico) + "  \n");
            return lstGrafico;
        }

        [Authorize]
        [Route("AcquirerTable")]
        [HttpPost]
        public List<StatisticsPerAcquirerTable> EstadPorAdquirienteTbl(StatisticsRequest EstRequest)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + EstRequest + " \n");
            List<StatisticsPerAcquirerTable> lstGrafico = new List<StatisticsPerAcquirerTable>();
            using (StatisticsModels objModels = new StatisticsModels())
            {
                lstGrafico = objModels.StatisticsPerEachAcquirer(EstRequest);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstGrafico) + "  \n");
            return lstGrafico;
        }

        [Authorize]
        [Route("TerminalTable")]
        [HttpPost]
        public List<StatisticsPerCommerceTable> EstadPorTerminalTbl(StatisticsRequestRetailer EstRequestRetail)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + EstRequestRetail + " \n");
            List<StatisticsPerCommerceTable> lstGrafico = new List<StatisticsPerCommerceTable>();
            using (StatisticsModels objModels = new StatisticsModels())
            {
                lstGrafico = objModels.StatisticsPerTerminal(EstRequestRetail);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstGrafico) + "  \n");
            return lstGrafico;
        }
    }
}