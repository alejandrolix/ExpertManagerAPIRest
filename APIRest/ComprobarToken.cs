using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APIRest
{
    public class ComprobarToken : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string cabeceraAutorizacion = context.HttpContext.Request.Headers["Authorization"];
            string token = cabeceraAutorizacion?.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
                context.Result = new BadRequestObjectResult("no token");
        }
    }
}
