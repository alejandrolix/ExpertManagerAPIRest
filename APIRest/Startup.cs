using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
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

            string esContenedor = Environment.GetEnvironmentVariable("ES_CONTENEDOR");
            string cadenaConexionBd = "";

            if (esContenedor is null)
            {
                cadenaConexionBd = Configuration.GetConnectionString("ExpertManagerContext");
            }
            else
            {
                cadenaConexionBd = Environment.GetEnvironmentVariable("CAD_CONEXION_BD");
            }

            services.AddDbContext<ExpertManagerContext>(options => options.UseSqlServer(cadenaConexionBd));

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
            services.AddScoped<RepositorioTokensUsuario>();
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
