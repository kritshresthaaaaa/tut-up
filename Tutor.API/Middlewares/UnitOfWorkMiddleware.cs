using Newtonsoft.Json;
using System.Net;
using Tutor.Application.Common.Interfaces;
using Tutor.Application.Common.Model.Error;

namespace Tutor.API.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UnitOfWorkMiddleware> _logger;

        public UnitOfWorkMiddleware(
            RequestDelegate next,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<UnitOfWorkMiddleware> logger)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var reqType = context.Request.Method;
            bool isTransactionalRequest = reqType == "POST" || reqType == "PUT" || reqType == "DELETE";

            if (isTransactionalRequest)
            {
                await unitOfWork.CreateTransaction();
            }

            try
            {
                await _next(context);

                if (isTransactionalRequest)
                {
                    await unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                if (isTransactionalRequest)
                {
                    await unitOfWork.RollbackAsync();
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorModel(HttpStatusCode.InternalServerError, exception.Message);
            var jsonResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}