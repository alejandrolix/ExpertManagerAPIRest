using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public PermisosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: PermisosController
        [HttpGet]
        public async Task<List<Permiso>> Index()
        {
            List<Permiso> permisos = await _contexto.Permisos                                                        
                                                    .ToListAsync();

            return permisos;
        }        
    }
}
