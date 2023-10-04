using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using RdwVehiclesService.Configuration;
using VehicleSearch.Api.Middleware;
using VehicleSearch.Api.Resources;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = GenericMessages.SwaggerTitle, Version = "v1.0.0" });

                    options.AddSecurityDefinition(GenericMessages.ApiKey, new OpenApiSecurityScheme
                    {
                        Description = GenericMessages.ApiKeyHeaderMessage,
                        Type = SecuritySchemeType.ApiKey,
                        Name = GenericMessages.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });
                    var key = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = GenericMessages.ApiKey
                        },
                        In = ParameterLocation.Header
                    };
                    var requirement = new OpenApiSecurityRequirement
                    {
                        { key, new List<string>() }
                    };
                    options.AddSecurityRequirement(requirement);
                });

builder.Services.ConfigureRdwVehicleService(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Search API");
            c.RoutePrefix = string.Empty;
        });

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;
            await context.Response.WriteAsync(ErrorMessages.GeneralExceptionMessage);

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = context.Features.Get<ILogger>();
            logger?.LogError(exceptionHandlerPathFeature?.Error, exceptionHandlerPathFeature?.Error.Message);
        });
    });

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }