using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchEngineAPIs.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SearchEngineAPIs.Models;

namespace SearchEngineAPIs.Controllers
{
    public class SearchParamsController : ApiController
    {


        [MyAuthorizationFilter]
        public IHttpActionResult Get([FromUri]Models.Filter filter)
        {
            if (filter.format != null)
                filter.format = filter.format.ToLower();
            if (filter.format == "json" || filter.format == "xml")
            {
                DataTable dataTable = RetrieveSearchEngines();
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    // Return a response indicating no data found
                    return NotFound();
                }
                var root = new JObject();
                root.Add("google_search", JToken.FromObject(dataTable));
                string json = JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                // Creates an JSON response
                var unserializedContent = JsonConvert.DeserializeXmlNode(json, "Searchengines");
                if (filter.format == "json")
                {
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };
                    // Return the JSON response
                    return Json(unserializedContent, serializerSettings);
                }
                if (filter.format == "xml")
                {
                    // Creates an XML response
                    string xmlString = unserializedContent.OuterXml;
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(xmlString, System.Text.Encoding.UTF8, "application/xml");
                    // Return the XML response
                    return ResponseMessage(response);
                }
                return NotFound();
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
        public DataTable RetrieveSearchEngines()
        {
            string conStr = ConfigurationManager.ConnectionStrings["TrackingKeywordsConnection"].ToString();
            SqlConnection cn = new SqlConnection(conStr);
            cn.Open();
            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT seid,domain,locale,geo_location,uule,device,sename FROM SearchEnginesList", cn))
            {
                da.Fill(dt);
            }
            cn.Close();
            return dt;
        }
    }
}
