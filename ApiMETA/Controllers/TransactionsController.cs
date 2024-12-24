using ApiMETA.Models;
using ApiMETA.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiMETA.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionsController : ApiController
    {
        [Authorize]
        [Route("Type")]
        [HttpGet]
        public List<TipoTransaccion> TipoTransaccion(string Usuario)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: Usuario=" + Usuario + "  \n");
            List<TipoTransaccion> lstTransaccion = new List<TipoTransaccion>();
            using (TransactionsModels objModels = new TransactionsModels())
            {
                lstTransaccion = objModels.SelectTipoTransaccion(Usuario);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstTransaccion) + "  \n");
            return lstTransaccion;
        }

        [Authorize]
        [Route("Search")]
        [HttpPost]
        public List<TransaccionInfo> BuscarTransacciones(TransaccionRequest tr)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + tr + "  \n");
            List<TransaccionInfo> lstTransaccion = new List<TransaccionInfo>();
            using (TransactionsModels objModels = new TransactionsModels())
            {
                lstTransaccion = objModels.SelectTransacciones(tr);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(lstTransaccion) + "  \n");
            return lstTransaccion;
        }

        [Authorize]
        [Route("Get")]
        [HttpPost]
        public TransaccionInfo GetTransaccion(TransaccionGet tg)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start: " + tg + "  \n");
            TransaccionInfo objTransaccion = new TransaccionInfo();
            using (TransactionsModels objModels = new TransactionsModels())
            {
                objTransaccion = objModels.GetTransaccion(tg);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + JsonConvert.SerializeObject(objTransaccion) + "  \n");
            return objTransaccion;
        }

        [Authorize]
        [Route("SendVoucher")]
        [HttpPost]
        public bool EnviarVoucher(VoucherRequest email)
        {
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Start:" + JsonConvert.SerializeObject(email) + "  \n");
            bool resp = false;
            using (TransactionsModels objTransModels = new TransactionsModels())
            {
                resp = objTransModels.SendVoucher(email);
            }
            Globals.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " INFO " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": End: " + resp + "  \n");
            return resp;
        }
    }
}