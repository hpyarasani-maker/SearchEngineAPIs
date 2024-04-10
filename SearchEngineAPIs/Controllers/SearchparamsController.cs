using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http;
using System.Web.Routing;
namespace SearchEngineAPIs.Controllers
{
    public class SearchparamsController : ApiController
    {
        [MyAuthorizationFilter]
        public IHttpActionResult Get([FromUri]Filter filter)
        {

        }
        public DataTable retrieveSearchEngines()
        {
            string conStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

            SqlConnection cn = new SqlConnection(conStr);
            cn.Open();
            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SearchEnginesList", cn))
            {
                da.Fill(dt);
            }
            cn.Close();
            return dt;
        }

    }
}
