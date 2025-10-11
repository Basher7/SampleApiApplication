using Domain.ResponseModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApiApplication.ActionFilters;

public class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            Type validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            IValidator? validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is null)
                continue;

            var validationContext = new ValidationContext<object>(argument);
            ValidationResult result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                var firstError = result.Errors.First();
                context.Result = new BadRequestObjectResult(new GlobalApiResponse(false, firstError.ErrorMessage));
                return;
            }
        }

        await next();
    }
}
