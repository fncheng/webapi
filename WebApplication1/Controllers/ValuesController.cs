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

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        public const string constr = @"Data Source=.;Initial Catalog=UserInfo;User ID=dongcheng;Password=Aa336699";
        // GET api/values
        public string GetAll()
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
                User user = new Models.User();
                user.Id = Convert.ToInt16(row["Id"]);
                user.Name = Convert.ToString(row["Name"]);
                user.Sex = Convert.ToString(row["Sex"]);
                user.Age = Convert.ToInt16(row["Age"]);

                userInfoList.Add(user);
            }
            conn.Close();
            return JsonConvert.SerializeObject(userInfoList);
        }

        // GET api/values/5
        public string Get(string test)
        {
            //test = "王";
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                //string spName = "Select * from people where name='王'";
                string spName = "userinfo_sel";
                SqlParameter[] selpara =
                {
                    new SqlParameter("@name",test)
                };
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
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
                        Name = reader["Name"].ToString(),
                        Sex = reader["Sex"].ToString(),
                        Age = Convert.ToInt16(reader["Age"])
                    };
                    list.Add(user);
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        [HttpPost]
        // POST api/values
        public void Post([FromBody]string test1,string test2,string test3)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string text = "userinfo_add";
            SqlParameter[] addpara =
            {
                    new SqlParameter("@name",test1),
                    new SqlParameter("@sex",test2),
                    new SqlParameter("@age",test3)
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

        // PUT api/values/5
        public void Put(int id, [FromBody]int test,string test1, string test2, string test3)
        {
            SqlConnection conn = new SqlConnection(constr);
            string text = "userinfo_update";
            SqlParameter[] updatepara =
            {
                new SqlParameter("@id",test),
                new SqlParameter("@name",test1),
                new SqlParameter("@sex",test2),
                new SqlParameter("@age",test3)
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

        // DELETE api/values/5
        public void Delete(int id)
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
