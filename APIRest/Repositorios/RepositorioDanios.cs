using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioDanios
    {
        private ExpertManagerContext _contexto;

        public RepositorioDanios(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }
    }
}
