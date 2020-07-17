using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using vintriTechnologies.Helper;

namespace vintriTechnologies
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Settings config = new Settings()
            {
                punkapiBaseUrl= Configuration["punkapiBaseUrl"],
                dbJsonPath = System.IO.Path.Combine(_env.ContentRootPath, Configuration["dbJsonPath"])   
            } ;

            services.AddControllers(); 
            services.AddHttpClient(); 
            
            services.AddSingleton<Settings, Settings>(a => config); 

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(Filters.ModelStateValidationFilter));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "This is my Swagger documentation for Vintri Technologies", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                foreach (var name in Directory.GetFiles(basePath, "*.XML", SearchOption.AllDirectories))
                {
                    c.IncludeXmlComments(name);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger documentation V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
