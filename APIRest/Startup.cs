using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Context;
using APIRest.Repositorios;

namespace APIRest
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
            services.AddMvc(opciones => opciones.Filters.Add(new GeneradorErrorRespuesta()));

            services.AddCors(options =>
            {
                options.AddPolicy("corsapi",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers(config => config.Filters.Add(new ComprobarToken()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIRest", Version = "v1" });
            });
            services.AddDbContext<ExpertManagerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ExpertManagerContext")));

            services.AddScoped<RepositorioSiniestros>();
            services.AddScoped<RepositorioEstados>();
            services.AddScoped<RepositorioAseguradoras>();
            services.AddScoped<RepositorioUsuarios>();
            services.AddScoped<RepositorioPeritos>();
            services.AddScoped<RepositorioDanios>();
            services.AddScoped<RepositorioPermisos>();
            services.AddScoped<RepositorioMensajes>();
            services.AddScoped<RepositorioDocumentaciones>();
            services.AddScoped<RepositorioImagenes>();
            services.AddScoped<RepositorioTiposArchivos>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIRest v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();            

            app.UseCors("corsapi");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
