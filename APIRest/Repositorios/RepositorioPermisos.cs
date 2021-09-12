using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public enum TipoPermiso
    {
        Administracion = 1,
        PeritoResponsable = 2,
        PeritoNoResponsable = 3
    }

    public class RepositorioPermisos
    {
        private ExpertManagerContext _contexto;

        public RepositorioPermisos(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }
    }
}
