using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SearchEngineAPIs.Models;
using SearchEngineAPIs.Filters;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Collections;
using System.Configuration;

namespace SearchEngineAPIs.Controllers
{
    public class NewsearchController : ApiController
    {
        [MyAuthorizationFilter]
        public IHttpActionResult Get([FromUri]Filter filter)
        {

            ArrayList al = new ArrayList();
            ArrayList al2 = new ArrayList();
            ArrayList msg = new ArrayList();
            int code1 = 0;
            if (filter.Source != null && filter.market != null && filter.language != null && filter.Device != null && filter.Location != null)
            {
                SqlConnection conn = null;
                SqlDataReader rdr = null;

                try
                {
                    string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                    //conn = new SqlConnection("Data Source=pi-software-13;Initial Catalog=SearchParams;User ID=sa;password=user13");
                    conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetGeoTargets", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@market", filter.market));
                    cmd.Parameters.Add(new SqlParameter("@language", filter.language));
                    cmd.Parameters.Add(new SqlParameter("@location", filter.Location));
                    cmd.Parameters.Add(new SqlParameter("@device", filter.Device));
                    DataTable dt = new DataTable();
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    code1 = HttpContext.Current.Response.StatusCode;
                    dt.Load(rdr);
                    foreach (DataRow row in dt.Rows)
                    {
                        msg.Add(row[0]);
                        string t = row[6].ToString();
                        if (t != "adult")
                        {
                            al.Add(row[0]);
                            al2.Add(row[5]);
                        }

                    }

                }

                catch (Exception ex)
                {
                }
                if (al.Count > 0 && al2.Count > 0)
                {
                    List<string> rid = al.Cast<string>()
                                           .ToList();
                    List<string> rlocation = al2.Cast<string>()
                                         .ToList();

                    Locations.RootObject root = new Locations.RootObject()
                    {
                        meta = new Locations.Meta()
                        {
                            status = code1,
                            data = new Locations.Data()
                            {
                                Searchengines = new Locations.searchengines()
                                {
                                    id = rid,
                                    source = filter.Source,
                                    market = filter.market,
                                    language = filter.language,
                                    device = filter.Device,
                                    location = rlocation
                                }
                            }
                        }
                    };
                    string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           }).Replace("Searchengine", "Search-engine");
                    //json = json.Replace("searchengine", "search-engine");
                    var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };
                    return Json(unserializedContent, serializerSettings);
                }
                else
                {
                    List<string> rmsg = msg.Cast<string>()
                                         .ToList();
                    Locations.RootObject root = new Locations.RootObject()
                    {
                        meta = new Locations.Meta()
                        {
                            status = code1,
                            data = new Locations.Data()
                            {
                                message = rmsg
                            }
                        }
                    };
                    string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           });
                    //json = json.Replace("searchengine", "search-engine");
                    var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };

                    return Json(unserializedContent, serializerSettings);

                }
            }


            else
            {

                string message = "";
                int last;
                if (filter.market == null)
                {

                    message += " Market,";
                }
                if (filter.language == null)
                {

                    message += " Language,";
                }
                if (filter.Device == null)
                {

                    message += " Device,";
                }
                if (filter.Location == null)
                {

                    message += " Location,";
                }
                last = message.LastIndexOf(',');
                if (last > 0)
                {
                    message = message.Remove(last);
                }
                code1 = HttpContext.Current.Response.StatusCode;
                JObject job = new JObject(
               new JProperty("meta", new JObject(
                       new JProperty("status", code1), new JProperty("Data", new JObject(new JProperty("Message", "Please provide " + message + " in the current URL"))))));
                string json = JsonConvert.SerializeObject(job, Formatting.Indented);
                var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                return Json(unserializedContent, serializerSettings);
                //message += "Location";
            }

        }

        public string RunStoredProc()
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            string aaa = "";
            try
            {
                conn = new SqlConnection("Data Source=pi-software-13;Initial Catalog=SearchParams;User ID=sa;password=user13");
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetGeoTargets", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@market", "us"));
                cmd.Parameters.Add(new SqlParameter("@language", "en"));
                cmd.Parameters.Add(new SqlParameter("@location", "abc"));
                cmd.Parameters.Add(new SqlParameter("@device", "desktop"));

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    aaa = rdr[0].ToString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return aaa;
        }
    }
}
