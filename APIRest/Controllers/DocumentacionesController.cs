using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
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
    public class DocumentacionesController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private IWebHostEnvironment _webHostEnvironment;

        public DocumentacionesController(ExpertManagerContext contexto, IWebHostEnvironment webHostEnvironment)
        {
            _contexto = contexto;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: DocumentacionesController
        [HttpGet("{id}")]
        public async Task<List<Aseguradora>> Index(int id)
        {
            

            //List<Aseguradora> aseguradoras = await _contexto.Aseguradoras                                                        
            //                                                .ToListAsync();

            return null;
        }        
    }
}
