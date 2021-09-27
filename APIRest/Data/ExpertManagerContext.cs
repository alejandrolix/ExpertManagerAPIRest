using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Context
{
    public class ExpertManagerContext : DbContext
    {
        public ExpertManagerContext(DbContextOptions<ExpertManagerContext> options) : base(options)
        {
        }

        public DbSet<Aseguradora> Aseguradoras { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Danio> Danios { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }        
        public DbSet<Siniestro> Siniestros { get; set; }                
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<TipoArchivo> TiposArchivo { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
    }
}
