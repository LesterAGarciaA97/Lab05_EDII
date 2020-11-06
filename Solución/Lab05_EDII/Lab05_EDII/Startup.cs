using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Lab05_EDII
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab05_EDII", Version = "v1", Description = "<center><h1>Cifrado Zig Zag, Cesar, Ruta Espiral</h1><h2>Hector Zetino 1295617<br>Lester Garcia 1003115</h2><h3>Funcionalidad Completa desde Swagger!!!!<br></br><br>Para su funcionalidad correcta desde swagger se debe de desmarcar las casillas 'Send empty value' a los valores que no se enviaran para el cifrado elegido</br></h3></center>" });
                var filepath = Path.Combine(Environment.CurrentDirectory, "Lab05_EDII.xml");
                c.IncludeXmlComments(filepath);
            });
        }

         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab05_EDII");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}