using isRock.Framework.WebAPI;
using System.Data.SqlClient;
using System.Data;
using System.Net.Cache;
using System.Management.Automation.Tracing;
using System.CodeDom;

namespace PTBO
{
    internal class AppControllers
    {
        public class LoginSearch
        {
            public string UserId { get; set; }
            public string Password { get; set; }
        }
        public class RegistreSearch
        {
            public string UserId { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Sex { get; set; }
            public string Phone { get; set; }
            public string BirthDay { get; set; }
            public string Address { get; set; }
        }
        public class AddShopCar
        {
            public string UserId { get; set; }
            public string ProId { get; set; }
            public int Num { get; set; }
        }
        public class DelShopCar
        {
            public string UserId { get; set; }
            public string ProId { get; set; }
        }
        public class UserInfo
        {
            public string UserId { get; set; }
        }
        internal class AppController
        {
            #region 登入
            public ExecuteCommandDefaultResult Login(LoginSearch request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT COUNT(*) FROM SysUserData WHERE UserId='{0}' AND Password = '{1}' ", request.UserId,request.Password);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count > 0)
                {
                    string SqlGrama2 = string.Format(@" SELECT * FROM SysUserData WHERE UserId='{0}' AND Password = '{1}' ", request.UserId, request.Password);

                    dataAdapter = new SqlDataAdapter(SqlGrama2, connection);//從Command取得資料存入dataAdapter

                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                }

                if (dt2.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt2
                };
            }
            #endregion

            #region 註冊
            public ExecuteCommandDefaultResult Registre(RegistreSearch request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT * FROM SysUserData WHERE UserId='{0}' ", request.UserId);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count == 0)
                {
                    string SqlGrama2 = string.Format(@" INSERT INTO SysUserData VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') "
                                        , request.UserId,request.Password,request.UserName,request.Phone,request.Sex,request.Address,request.BirthDay);

                    dataAdapter = new SqlDataAdapter(SqlGrama2, connection);//從Command取得資料存入dataAdapter

                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 抓商品資料
            public ExecuteCommandDefaultResult GetProductList()
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT * FROM Products ");

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 加入購物車
            public ExecuteCommandDefaultResult SetShopCar(AddShopCar request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                int Num = 0;
                var tempNum = new object();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT * FROM ShopCar WHERE ProId = '{0}' AND UserId = '{1}' ", request.ProId, request.UserId);
                
                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds, "ShopCar");
                
                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow pRow in ds.Tables["ShopCar"].Rows)
                    {
                        tempNum = pRow["Num"];
                    }
                    Num = (int)tempNum + request.Num;
                    SqlGrama = string.Format(@"UPDATE ShopCar SET Num = '{2}' WHERE ProId ='{0}' AND UserId = '{1}' ", request.ProId, request.UserId, Num);
                    dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    SqlGrama = string.Format(@"INSERT INTO ShopCar VALUES ('{0}','{1}',{2})", request.ProId, request.UserId, request.Num);
                    dt = null;
                    dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    isSuccess = true;
                    message = "成功";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 修改購物車數量
            public ExecuteCommandDefaultResult UpdateShopCar(AddShopCar request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                int Num = 0;
                var tempNum = new object();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;

                string SqlGrama = string.Format(@"UPDATE ShopCar SET Num = '{2}' WHERE ProId = '{0}' AND UserId = '{1}' ", request.ProId, request.UserId,request.Num);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                SqlGrama = string.Format(@"SELECT * FROM ShopCar WHERE ProId = '{0}' AND UserId = '{1}' AND Num = '{2}' ", request.ProId, request.UserId, request.Num);

                dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset

                if (dt2.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    SqlGrama = string.Format(@"INSERT INTO ShopCar VALUES ('{0}','{1}',{2})", request.ProId, request.UserId, request.Num);
                    dt = null;
                    dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    isSuccess = true;
                    message = "成功";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt2
                };
            }
            #endregion

            #region 檢視購物車
            public ExecuteCommandDefaultResult GetShopCar(UserInfo request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT * FROM V_ShopCar WHERE UserId = '{0}'",request.UserId);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 刪除購物車
            public ExecuteCommandDefaultResult DelShopCar(DelShopCar request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"DELETE FROM ShopCar WHERE UserId = '{0}' AND ProId = '{1}' ", request.UserId,request.ProId);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                SqlGrama = string.Format(@"SELECT * FROM ShopCar WHERE UserId = '{0}' AND ProId = '{1}' ", request.UserId, request.ProId);

                dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                if (dt2.Rows.Count == 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 結帳
            public ExecuteCommandDefaultResult SetShopPay(AddShopCar request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                int Num = 0;var tempNum = new object(); 
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                connection.Open();
                string SqlGrama2 = string.Format(@"SELECT * FROM Products WHERE ProId='{0}'", request.ProId);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama2, connection);//從Command取得資料存入dataAdapter
                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "Products");
                foreach (DataRow pRow in ds.Tables["Products"].Rows)
                {
                    tempNum = pRow["Stock"];
                }
                Num = (int)tempNum - request.Num;
                connection.Close();
                if (dt.Rows.Count > 0)
                {
                    connection.Open();
                    string SqlGrama = string.Format(@"INSERT INTO ShopPay VALUES ('{0}','{1}',{2})", request.ProId, request.UserId, request.Num);
                    dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    string SqlGrama3 = string.Format(@"UPDATE Products SET Stock = '{1}' WHERE ProId = '{0}'", request.ProId, Num);
                    dataAdapter = new SqlDataAdapter(SqlGrama3, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    SqlGrama3 = string.Format(@"DELETE FROM ShopCar WHERE ProId = '{0}' AND UserId = '{1}'", request.ProId, request.UserId);
                    dataAdapter = new SqlDataAdapter(SqlGrama3, connection);//從Command取得資料存入dataAdapter
                    dataAdapter.Fill(dt2);//將dataAdapter資料存入dataset
                    isSuccess = true;
                    message = "成功";
                    connection.Close();
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

            #region 檢視結帳
            public ExecuteCommandDefaultResult GetShopPay(UserInfo request)
            {
                var message = "";
                bool isSuccess = false;
                DataTable dt = new DataTable();
                string s_data = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = s_data;
                string SqlGrama = string.Format(@"SELECT * FROM V_ShopPay WHERE UserId = '{0}'", request.UserId);

                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlGrama, connection);//從Command取得資料存入dataAdapter

                dataAdapter.Fill(dt);//將dataAdapter資料存入dataset

                if (dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功";
                }
                else
                {
                    isSuccess = false;
                    message = "失敗";
                }
                dataAdapter.Dispose();
                connection.Close();
                return new ExecuteCommandDefaultResult()
                {
                    isSuccess = isSuccess,
                    Message = message,
                    Data = dt
                };
            }
            #endregion

        }
    }
}