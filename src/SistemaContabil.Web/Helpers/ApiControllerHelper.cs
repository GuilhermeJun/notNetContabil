using Microsoft.AspNetCore.Mvc;

namespace SistemaContabil.Web.Helpers;

/// Helper para padronizar respostas de API
public static class ApiControllerHelper
{
    public static ProblemDetails CreateProblemDetails(
        int statusCode,
        string title,
        string detail,
        string? instance = null,
        IDictionary<string, object?>? extensions = null)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = instance
        };

        if (extensions != null)
        {
            foreach (var extension in extensions)
            {
                problem.Extensions.Add(extension.Key, extension.Value);
            }
        }

        return problem;
    }

    public static ActionResult BadRequestProblem(string detail, string? instance = null)
    {
        return new ObjectResult(CreateProblemDetails(
            StatusCodes.Status400BadRequest,
            "Erro de validação",
            detail,
            instance))
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    public static ActionResult NotFoundProblem(string detail, string? instance = null)
    {
        return new ObjectResult(CreateProblemDetails(
            StatusCodes.Status404NotFound,
            "Recurso não encontrado",
            detail,
            instance))
        {
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public static ActionResult InternalServerErrorProblem(string detail, string? instance = null)
    {
        return new ObjectResult(CreateProblemDetails(
            StatusCodes.Status500InternalServerError,
            "Erro interno do servidor",
            detail,
            instance))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
