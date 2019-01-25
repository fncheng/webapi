using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using WebApplication1.Models;
using System.Web.Http.Results;
using System.Xml;
using System.Xml.Serialization;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        public const string constr = @"Data Source=.;Initial Catalog=UserInfo;User ID=dongcheng;Password=Aa336699";
        // GET api/values
        /// <summary>
        /// 客户端调用某个Action方法并希望以JSON的格式返回请求的数据，使用JsonResult
        /// </summary>
        /// <returns></returns>
        public /*string*/JsonResult<List<User>> GetAll()
        {
            List<User> userInfoList = new List<User>();
            SqlConnection conn = new SqlConnection(constr);
            string text = "userinfo_get";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(text, conn);
            DataSet ds = new DataSet("Students");
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                User user = new User
                {
                    Id = Convert.ToInt16(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    Sex = Convert.ToString(row["Sex"]),
                    Age = Convert.ToInt16(row["Age"])
                };

                userInfoList.Add(user);
            }
            conn.Close();
            return Json<List<User>>(userInfoList);
            ///xml不可行
            //XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            //return serializer.Serialize(userInfoList);
            
            //return JsonConvert.SerializeObject(userInfoList);
        }

        // GET api/values/5
        public /*string*/JsonResult<List<User>> Get(string name)
        {
            //test = "王";
            //try
            //{
                SqlConnection conn = new SqlConnection(constr);
                //string spName = "Select * from people where name='王'";
                string spName = "userinfo_sel";
                SqlParameter[] selpara =
                {
                    new SqlParameter("@name",name)
                };
                SqlCommand cmd = new SqlCommand(spName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                foreach (SqlParameter parameter in selpara)
                {
                    cmd.Parameters.Add(parameter);
                }
                SqlDataReader reader = cmd.ExecuteReader();
                List<User> list = new List<User>();
                while (reader.Read())
                {
                    User user = new User()
                    {
                        Id = Convert.ToInt16(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Sex = reader["Sex"].ToString(),
                        Age = Convert.ToInt16(reader["Age"])
                    };
                    list.Add(user);
                }
                return Json<List<User>>(list);
                //return JsonConvert.SerializeObject(list);
            //}
            //catch (Exception ex)
            //{
            //    return Json<List<User>>("1");
            //}
        }
        [HttpPost]
        // POST api/values
        public void Add([FromBody]Para para)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string text = "userinfo_add";
            SqlParameter[] addpara =
            {
                    new SqlParameter("@name",para.Name),
                    new SqlParameter("@sex",para.Sex),
                    new SqlParameter("@age",para.Age)
                };
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in addpara)
            {
                if (parameter != null)
                    cmd.Parameters.Add(parameter);
                else
                    cmd.Parameters.Add(0);
            }
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        [HttpPost]
        // PUT api/values/5
        public void Update([FromBody]Para para)
        {
            SqlConnection conn = new SqlConnection(constr);
            string text = "userinfo_update";
            SqlParameter[] updatepara =
            {
                new SqlParameter("@id",para.Id),
                new SqlParameter("@name",para.Name),
                new SqlParameter("@sex",para.Sex),
                new SqlParameter("@age",para.Age)
            };
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in updatepara)
            {
                if (parameter != null)
                    cmd.Parameters.Add(parameter);
                else
                    continue;
            }
            conn.Open();    //Open the sql
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        [HttpPost]
        // DELETE api/values/5
        public void Delete([FromBody]int id)
        {
            SqlConnection conn = new SqlConnection(constr);
            string text = "userinfo_del";
            SqlParameter[] delpara = {
                new SqlParameter("@id",id)
            };
            conn.Open();    //Open the sql
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in delpara)
            {
                cmd.Parameters.Add(parameter);
            }

            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
