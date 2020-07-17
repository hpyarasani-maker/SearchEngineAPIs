using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SearchEngineAPIs.Filters;
using SearchEngineAPIs.Models;

namespace SearchEngineAPIs.Controllers
{
    public class SourcetypeController : ApiController
    {
        [MyAuthorizationFilter]
        //Retrieve Sourcetype for given parameters
        public IHttpActionResult Get([FromUri]Filter filter) 
        {
            DataTable se = retrieveSearchEngines();
            int code1;

            if (filter.Source != null && filter.market != null)
            {
                filter.market = filter.market.ToLower();
                filter.Source = filter.Source.ToLower();                
                var exprn="";
                if (filter.type!=null)
                {
                    exprn = "Source='" + filter.Source + "' And market='" + filter.market + "' And type='" + filter.type + "' ";
                }
                else
                {
                    exprn = "Source='" + filter.Source + "' And market='" + filter.market + "' And type is null ";
                }
                DataRow[] result = se.Select(exprn);
                code1 = HttpContext.Current.Response.StatusCode;
                ArrayList al = new ArrayList();
                ArrayList a2 = new ArrayList();
                ArrayList a3 = new ArrayList();
                foreach (DataRow row in result)
                {
                    al.Add(row[0]);
                    a2.Add(row[5]);
                    a3.Add(row[1]);                    
                }
                if (al.Count > 0)
                {
                    List<string> rlocation = a2.Cast<string>()
                                         .ToList();
                    Locations.RootObject root = new Locations.RootObject()
                    {
                        meta = new Locations.Meta()
                        {
                            status =  code1,//JSON.parse(data)

                            data = new Locations.Data()
                            {
                                    locations = rlocation,
                                    source = filter.Source,
                                    market = filter.market,                                  
                            }
                        }
                    };
                    string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None,
                          new JsonSerializerSettings
                          {
                              NullValueHandling = NullValueHandling.Ignore
                          });
                    var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };

                    return Json(unserializedContent, serializerSettings);
                    
                }
                else
                {    //Error response if the parameter is invalid  
                    locationError.RootObject root = new locationError.RootObject()
                    {
                        meta = new locationError.Meta()
                        {
                            status = 104,
                            errors = new locationError.Errors()
                            {                                
                                message = "Invalid Parameter"
                            }
                        }
                    };
                    string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None,
                       new JsonSerializerSettings
                       {
                           NullValueHandling = NullValueHandling.Ignore
                       });
                    var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };                    
                    return Json(unserializedContent, serializerSettings);
                }
            }
            else
            {
                //Error response if the code is invalid
                code1 = HttpContext.Current.Response.StatusCode;
                locationError.RootObject root = new locationError.RootObject()
                {
                    meta = new locationError.Meta()
                    {
                        status = code1,
                        errors = new locationError.Errors()
                        {
                            message = "The code is invalid"
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None,
                   new JsonSerializerSettings
                   {
                       NullValueHandling = NullValueHandling.Ignore
                   });
                var unserializedContent = JsonConvert.DeserializeXmlNode(json);
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };

                return Json(unserializedContent, serializerSettings);
              
            }
        }
        public DataTable retrieveSearchEngines()
        {
            string conStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            
            SqlConnection cn = new SqlConnection(conStr);
            cn.Open();
            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SearchEnginesParametersTest", cn))
            {
                da.Fill(dt);
            }
            cn.Close();
            return dt;
        }
        
    }
}
