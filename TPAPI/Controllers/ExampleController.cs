using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using isRock.Framework;
using isRock.Framework.WebAPI;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace TPAPI.Controllers
{
    public class ExampleController : ApiController
    {
        [Route("api/Example/{MethodName}")]
        [HttpPost]
        public HttpResponseMessage ExecuteMethod()
        {
            var message = "";
            bool isSuccess;
            DataTable dt = new DataTable();
            string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = s_data;
            string SqlGrama = string.Format(@"SELECT TOP (10) *  FROM V_PLInventoryList");

            connection.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

            DataSet dataset = new DataSet();//創一個dataset的記憶體資料集

            dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }
    }
        

    #region "這是樣板，實際BusinessLogic不該寫在Controller這裡，請移到獨立的Class"
    //回傳參數(也可以視為ViewModel)
    public class TestMethodResut
    {
        public int value1 { get; set; }
        public string value2 { get; set; }
    }
	//傳入參數
    public class TestMethodParameter
    {
        public int ValueA { get; set; }
        public string ValueB { get; set; }
    }
	//必須繼承 BusinessLogicBase
    public class TestClassA : BusinessLogicBase
    {
	    //請注意，務必只能有一個傳入參數
        public ExecuteCommandDefaultResult TestMethodA(TestMethodParameter para)
        {
            return new ExecuteCommandDefaultResult
            {
                Data = new TestMethodResut() { value1 = para.ValueA, value2 = para.ValueB },
                isSuccess = true,
                Message = ""
            };
        }
    }
	#endregion
}
