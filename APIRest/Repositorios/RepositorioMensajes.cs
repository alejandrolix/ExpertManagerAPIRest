using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;

namespace APIRest.Repositorios
{
    public class RepositorioMensajes
    {
        private ExpertManagerContext _contexto;

        public RepositorioMensajes(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Mensaje>> ObtenerTodosPorIdSiniestro(int id)
        {
            List<Mensaje> mensajes = await _contexto.Mensajes
                                                    .Include(mensaje => mensaje.Siniestro)
                                                    .Include(mensaje => mensaje.Usuario)
                                                    .Where(mensaje => mensaje.Siniestro.Id == id)
                                                    .ToListAsync();
            return mensajes;
        }        

        public async Task Guardar(Mensaje mensaje)
        {
            try
            {
                _contexto.Add(mensaje);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
