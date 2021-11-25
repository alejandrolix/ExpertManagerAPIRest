using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;
using APIRest.Excepciones;
using System.Net;

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
            if (mensajes is null)
                throw new CodigoErrorHttpException("No existen mensajes", HttpStatusCode.NotFound);

            return mensajes;
        }

        public async Task<Mensaje> ObtenerPorId(int id)
        {
            Mensaje mensaje = await _contexto.Mensajes
                                             .FirstOrDefaultAsync(mensaje => mensaje.Id == id);
            return mensaje;
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

        public async Task Eliminar(Mensaje mensaje)
        {
            try
            {
                _contexto.Remove(mensaje);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
