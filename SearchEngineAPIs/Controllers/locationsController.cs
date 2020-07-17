using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SearchEngineAPIs.Filters;
using SearchEngineAPIs.Models;

namespace SearchEngineAPIs.Controllers

{ 
    public class locationsController : ApiController
    {
        //Retrieve Available Locations for a given Market and Source
        
        [MyAuthorizationFilter]
    
    public IHttpActionResult Get([FromUri]Filter filter)       
         {
           DataTable se = retrieveSearchEngines();
            if (filter.market != null && filter.Source != null)
            {
                filter.market = filter.market.ToLower();
                filter.Source = filter.Source.ToLower();
                var code1 = HttpContext.Current.Response.StatusCode;
                var exprn = "market='" + filter.market + "' And Source='" + filter.Source + "'";                
                DataRow[] result = se.Select(exprn);           
                 ArrayList al = new ArrayList();               
                foreach (DataRow row in result)
                {
                    if (!al.Contains(row[5]))
                    {
                        al.Add(row[5]);
                    }                    
                }                
                if (al.Count > 0)
                {                   

                    List<string> results = al.Cast<string>()
                                    .ToList();
                    Locations.RootObject root = new Locations.RootObject()
                    {
                        meta = new Locations.Meta()
                        {
                            status = code1,
                            data = new Locations.Data()
                            {
                                market = filter.market,
                                location = results
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
                //Error response if the Country name is Wrong
                else
                {
                    if (filter.market != null && filter.Source != null)
                    {
                      
                        locationError.RootObject root = new locationError.RootObject()
                        {
                            meta = new locationError.Meta()
                            {
                                status = code1,
                                errors = new locationError.Errors()
                                {
                                    code = 10,
                                    message = "The Country "+ filter.market + " is wrong"
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
                    //Error response if the parameter is not valid
                    else
                    {
                      locationError.RootObject root = new locationError.RootObject()
                        {
                            meta = new locationError.Meta()
                            {
                                status = 103,
                                errors = new locationError.Errors()
                                {                                    
                                    message = "Parameter missing or not valid"
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
            }
            //Error response if the parameter is missing
            else
            {
               
                locationError.RootObject root = new locationError.RootObject()
                {
                    meta = new locationError.Meta()
                    {
                        status = 103,
                        errors = new locationError.Errors()
                        {
                            message = "parameter is missing"
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
