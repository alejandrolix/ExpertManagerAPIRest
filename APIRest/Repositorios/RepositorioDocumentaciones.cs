using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;

namespace APIRest.Repositorios
{
    public class RepositorioDocumentaciones
    {
        private ExpertManagerContext _contexto;

        public RepositorioDocumentaciones(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }
    }
}
