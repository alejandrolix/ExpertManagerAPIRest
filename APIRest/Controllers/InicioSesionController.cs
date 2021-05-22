using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class InicioSesionController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public InicioSesionController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        public async Task<JsonResult> IniciarSesion(UsuarioVm usuarioVm)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .FirstOrDefaultAsync(usuario => usuario.Nombre == usuarioVm.Nombre &&
                                                                  usuario.Contrasenia == usuarioVm.HashContrasenia);
            if (usuario is null)
                return new JsonResult(false);
            else
            {
                UsuarioVm respuesta = new UsuarioVm()
                {
                    Nombre = usuario.Nombre,
                    IdPermiso = usuario.Permiso.Id
                };

                return new JsonResult(respuesta);
            }                               
        }        
    }
}
