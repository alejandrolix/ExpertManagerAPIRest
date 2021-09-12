using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioUsuarios
    {
        private ExpertManagerContext _contexto;

        public RepositorioUsuarios(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Where(usuario => usuario.Id == id)
                                             .FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<int> ObtenerNumSiniestrosPorIdUsuario(int id)
        {
            int numSiniestros = await _contexto.Siniestros
                                               .Include(siniestro => siniestro.UsuarioCreado)                                               
                                               .Where(siniestro => siniestro.UsuarioCreado.Id == id)
                                               .CountAsync();
            return numSiniestros;
        }
    }
}
