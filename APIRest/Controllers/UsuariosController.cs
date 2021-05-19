using APIRest.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.ViewModels;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public UsuariosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<List<UsuarioVm>> ObtenerTodos()
        {
            List<Usuario> usuarios = await _contexto.Usuarios
                                                    .Include(usuario => usuario.Permiso)
                                                    .OrderBy(usuario => usuario.Nombre)
                                                    .ToListAsync();

            //List<UsuarioVm> usuariosVms = usuarios.Select(usuario => new UsuarioVm(usuario)).ToList();

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> Create(UsuarioVm usuarioVm)
        {
            try
            {
                List<Aseguradora> aseguradoras = new List<Aseguradora>
                {
                    new Aseguradora()
                };
                Aseguradora[] aseguradoras2 = new Aseguradora[]
                {
                    new Aseguradora()
                };

                var kk = aseguradoras.Select(a => a.Id).Union(aseguradoras2.Select(a => a.Id)).ToList();

                bool esPerito;

                Permiso permiso = await _contexto.Permisos
                                                 .FirstOrDefaultAsync(permiso => permiso.Id == usuarioVm.IdPermiso);

                if (usuarioVm.IdEsPerito == 0)
                    esPerito = true;
                else
                    esPerito = false;

                Usuario usuario = new Usuario()
                {
                    Nombre = usuarioVm.Nombre,
                    EsPerito = esPerito,
                    Contrasenia = usuarioVm.hashContrasenia,
                    Permiso = permiso
                };

                _contexto.Add(usuario);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }
    }
}
