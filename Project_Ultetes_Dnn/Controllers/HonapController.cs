using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Controllers
{
    public class HonapController : DnnController
    {
        // Ez lesz az új modulod alapértelmezett nézete
        public ActionResult Index()
        {
            return View();
        }
    }
}