using isRock.Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TPAPI.Controllers
{
    public class AppController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MethodName">要執行的方法</param>
        /// <returns></returns>
        /// 
        //[OnlyReqHttps]
        //[Authorize]
        [Route("App/{MethodName}")]
        [HttpPost]
        //[HttpGet]     
        public IHttpActionResult ExecuteMethod(string MethodName)
        {
            try
            {
                //AssemblyLauncher
                AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
                //執行指定的Method

                var ret = assemblyLauncher.ExecuteCommand<PTBO.AppControllers.AppController>(
                    new PTBO.AppControllers.AppController(),
                    MethodName,
                   Request.Content.ReadAsStringAsync().Result
                    );
                //回傳OK
                return Ok(ret);
            }
            catch (Exception ex)
            {
                //其他處理
                throw ex;
            }
        }
    }
}
        