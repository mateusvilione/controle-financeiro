using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

using Api.DBContext;
using Api.Repository;
using Api.Services;
using Api.Entities;
using Api.Models;

namespace Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ControleFinanceiroContext>(Configuration.GetConnectionString("DB"), HealthStatus.Unhealthy);

            services
                .AddControllers()
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(setupAction =>
                 {
                     setupAction.InvalidModelStateResponseFactory = context =>
                     {
                         var problemDetailsFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ProblemDetailsFactory>();

                         var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                                 context.HttpContext,
                                 context.ModelState);

                         if (context.ModelState.ErrorCount > 0 && problemDetails.Status.Equals(StatusCodes.Status400BadRequest))
                         {
                             List<String> errors = new List<string>();
                             foreach (var (key, value) in context.ModelState)
                             {
                                 if (value.Errors.Count > 0 && value.Errors[0] != null)
                                 {
                                     if (value.Errors[0].ErrorMessage.StartsWith("Could not convert") ||
                                         value.Errors[0].ErrorMessage.StartsWith("Error converting value") ||
                                         value.Errors[0].ErrorMessage.StartsWith("The JSON value could not be converted"))
                                     {
                                         errors.Add($"{key.Replace("$.", "")} tipo de informação inválida");
                                     }
                                     else
                                     {
                                         errors.Add(value.Errors[0].ErrorMessage);
                                     }
                                 }
                             }
                             return new BadRequestObjectResult(new ErroModel("erro_validacao", string.Join("; ", errors.Reverse<string>().ToList())));
                         }

                         return new StatusCodeResult(500);
                     };
                 });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "CRUD de Controle financeiro",
                        Version = "v1",
                        Description = "Serviço criado para processos com os Controle financeiro"
                    });
            });

            services.AddCors(c =>
                      {
                          c.AddPolicy("AllowOrigin",
                          options =>
                          options.WithOrigins("http://localhost:5000")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                          );
                      });

            services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            services.AddScoped<ICategoriaService, CategoriaService>();

            services.AddScoped<IPaginarService<CategoriaEntity>, PaginarService<CategoriaEntity>>();


            services.AddScoped<ISubcategoriaRepository, SubcategoriaRepository>();

            services.AddScoped<ISubcategoriaService, SubcategoriaService>();

            services.AddScoped<IPaginarService<SubcategoriaEntity>, PaginarService<SubcategoriaEntity>>();


            services.AddScoped<ILancamentoRepository, LancamentoRepository>();

            services.AddScoped<ILancamentoService, LancamentoService>();

            services.AddScoped<IPaginarService<LancamentoEntity>, PaginarService<LancamentoEntity>>();


            services.AddScoped<IBalancoRepository, BalancoRepository>();

            services.AddScoped<IBalancoService, BalancoService>();

            services.AddScoped<IPaginarService<BalancoEntity>, PaginarService<BalancoEntity>>();
            

            services.AddDbContext<ControleFinanceiroContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DB"));
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(c => c.WithOrigins("http://localhost:5000"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Serviço de inclusão de veículos");
            });

            app.UseEndpoints(endpoints =>
            {
                var healthStatusCodes = new Dictionary<HealthStatus, int>()
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                };

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    ResultStatusCodes = healthStatusCodes,
                    ResponseWriter = WriteResponse,
                    Predicate = (_) => false
                });

                endpoints.MapHealthChecks("/health/details", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    ResultStatusCodes = healthStatusCodes
                });
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteStartObject("results");
                    foreach (var entry in result.Entries)
                    {
                        writer.WriteStartObject(entry.Key);
                        writer.WriteString("status", entry.Value.Status.ToString());
                        writer.WriteString("description", entry.Value.Description);
                        writer.WriteStartObject("data");
                        foreach (var item in entry.Value.Data)
                        {
                            writer.WritePropertyName(item.Key);
                            JsonSerializer.Serialize(
                                writer, item.Value, item.Value?.GetType() ??
                                typeof(object));
                        }
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return context.Response.WriteAsync(json);
            }
        }
    }
}
