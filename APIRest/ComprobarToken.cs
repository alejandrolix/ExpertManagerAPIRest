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
            int numQueryStrings = context.HttpContext.Request.Query["token"].Count;

            if (numQueryStrings == 0)
                context.Result = new BadRequestObjectResult("no token");
        }
    }
}
