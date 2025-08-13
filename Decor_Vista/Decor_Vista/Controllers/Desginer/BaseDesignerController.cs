using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Decor_Vista.Controllers.Desginer
{
    public class BaseDesignerController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var imagePath = HttpContext.Session.GetString("DesignerImg");
            var fullName = HttpContext.Session.GetString("DesignerName");

            ViewBag.DesignerImage = string.IsNullOrEmpty(imagePath)
                ? "/Designer/assets/images/avatars/01.png"
                : imagePath;

            ViewBag.DesignerName = fullName;

            base.OnActionExecuting(context);
        }
    }
}
