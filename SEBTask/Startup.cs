using API.Attributes;
using API.Middleware;
using Domain.Contracts.DataAccess;
using Domain.Contracts.Services;
using Domain.Services;
using Infrastructure.DataAccess;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace SEBTask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<SEBTaskDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SEBTaskDbContext"))
                )
                .AddDataAcess()
                .AddDomainServices()
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "SEB task API", Version = "v1" });
                    options.DescribeAllEnumsAsStrings();

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                })
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ModelStateValidationAttribute));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCustomExceptionHandler()
                .UseHttpsRedirection()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(sa =>
                {
                    sa.SwaggerEndpoint("/swagger/v1/swagger.json", "SEB task API V1");
                });
        }
    }

    public static class ConfigurationExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            return app;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddHttpClient<IBaseRateService, BaseRateService>();
            services.AddScoped<IInterestRateService, InterestRateService>();

            return services;
        }

        public static IServiceCollection AddDataAcess(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
