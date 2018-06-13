using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Design.Controllers
{
    public class BaseController : Controller
    {
        public JsonResult JsonWrapper(Object data)
        {
            return new JsonResultWrapper()
            {
                Data = data,
                ContentType = "application/json"
            };
        }
    }

    public class JsonResultWrapper : JsonResult
    {
        public JsonResultWrapper()
            : base()
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(this.Data,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //ContractResolver=new DefaultContractResolver()
                    ContractResolver = new UpperCaseContractResolver()

                });
            response.Write(data);
        }

        public class UpperCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToUpper();
            }
        }
    }
}