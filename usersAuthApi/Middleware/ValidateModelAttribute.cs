using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace usersAuthApi.Middleware
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        // This method is called before the action executes
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the ModelState is invalid
            if (!context.ModelState.IsValid)
            {
                // If ModelState is invalid, return a BadRequest with validation errors
                context.Result = new BadRequestObjectResult(new
                {
                    Errors = context.ModelState
                        .Where(e => e.Value.Errors.Any())
                        .Select(e => new
                        {
                            Field = e.Key,
                            Errors = e.Value.Errors.Select(err => err.ErrorMessage)
                        })
                        .ToList()
                });
            }

            // Continue to the action if ModelState is valid
            base.OnActionExecuting(context);
        }
    }
}
