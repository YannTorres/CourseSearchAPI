using Azure;
using CourseSearch.Communication.Responses.Error;
using CourseSearch.Exception.ExceptionBase;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CourseSearch.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CourseSearchException)
        {
            var courseSearchException = context.Exception as CourseSearchException;
            var errors = new ResponseErrorJson(courseSearchException!.GetErrors());

            context.Result = new ObjectResult(new
            {
                Errors = errors
            });

            context.HttpContext.Response.StatusCode = courseSearchException.StatusCode;
        } else
        {
            var errorResponse = new ResponseErrorJson("Erro desconhecido");

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
