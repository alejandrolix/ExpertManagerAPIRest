using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioSiniestros
    {
        private ExpertManagerContext _contexto;

        public RepositorioSiniestros(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Siniestro> ObtenerPorId(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .Include(siniestro => siniestro.Aseguradora)
                                                 .Include(siniestro => siniestro.Estado)
                                                 .Include(siniestro => siniestro.UsuarioCreado)
                                                 .Include(siniestro => siniestro.Perito)
                                                 .Include(siniestro => siniestro.Danio)
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == id);
            return siniestro;
        }

        public async Task<List<Siniestro>> ObtenerPorIdPerito(int idPerito)
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                                        .Include(siniestro => siniestro.Aseguradora)
                                                        .Include(siniestro => siniestro.Estado)
                                                        .Include(siniestro => siniestro.UsuarioCreado)
                                                        .Include(siniestro => siniestro.Perito)
                                                        .Include(siniestro => siniestro.Danio)
                                                        .Where(siniestro => siniestro.Perito.Id == idPerito)
                                                        .ToListAsync();
            return siniestros;
        }

        public async Task<List<Siniestro>> ObtenerTodos()
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                                        .Include(siniestro => siniestro.Aseguradora)
                                                        .Include(siniestro => siniestro.Estado)
                                                        .Include(siniestro => siniestro.UsuarioCreado)
                                                        .Include(siniestro => siniestro.Perito)
                                                        .Include(siniestro => siniestro.Danio)
                                                        .ToListAsync();
            return siniestros;
        }
    }
}
