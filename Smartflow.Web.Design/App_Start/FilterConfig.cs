using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Design
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}