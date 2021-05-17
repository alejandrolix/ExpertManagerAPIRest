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

            List<UsuarioVm> usuariosVms = usuarios.Select(usuario => new UsuarioVm(usuario)).ToList();

            return usuariosVms;
        }
    }
}
