using System.Web.Mvc;

namespace SManagerWeb.Areas.Teacher
{
    public class TeacherAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Teacher";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Teacher_default",
                "Teacher/{controller}/{action}/{id}",
                new { action = "Index", controller = "Organization", id = UrlParameter.Optional },
                new string[] { "SManagerWeb.Areas.Teacher.Controllers" }
            );
        }
    }
}