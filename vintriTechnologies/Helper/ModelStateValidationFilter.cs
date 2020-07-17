using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using vintriTechnologies.Helper;

namespace vintriTechnologies.Filters
{
    /// <summary>
    /// Task 3: Add a custom Web API Action Filter Attribute
    /// </summary>
    public class ModelStateValidationFilter : Attribute,  IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            { 
                context.Result = new ValidationFailedResult(context.ModelState);
            } 
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
