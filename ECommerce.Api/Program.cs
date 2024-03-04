using ECommerce.Api.Extensions;
using ECommerce.Business.Extensions;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
await builder.Services.AddDataAccessServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseSentryTracing();

#region Hata yakalama
app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionObject != null)
            {
                context.Response.StatusCode = exceptionObject.Error switch
                {
                    BadRequestException ex => StatusCodes.Status400BadRequest,
                    NotFoundException ex => StatusCodes.Status404NotFound,
                    ForbiddenException ex => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                };
                var errorMessage = $"{exceptionObject.Error.Message}";
                if (context.Response.StatusCode >= 500)
                    errorMessage = "Sunucu tarafýnda beklenmeyen bir hata oluþtu! Tekrar deneyiniz.";

                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(new
                    {
                        context.Response.StatusCode,
                        ErrorMessage = errorMessage
                    }))
                    .ConfigureAwait(false);
            }
        });
    }
);
#endregion

//app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
