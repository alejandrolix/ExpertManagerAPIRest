using APIRest.Context;
using APIRest.Excepciones;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

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

        public async Task<Permiso> ObtenerPorId(int id)
        {
            Permiso permiso = await _contexto.Permisos
                                             .FirstOrDefaultAsync(permiso => permiso.Id == id);            
            if (permiso is null)
                throw new CodigoErrorHttpException($"No existe el permiso con id {id}", HttpStatusCode.NotFound);

            return permiso;
        }

        public bool TienePermisoAdministracion(int idPermiso)
        {
            int idPermisoAdministracion = (int)TipoPermiso.Administracion;

            if (idPermiso == idPermisoAdministracion)
                return true;

            return false;
        }

        public bool EsPeritoNoResponsable(int idPermiso)
        {
            int idPermisoPeritoNoResponsable = (int)TipoPermiso.PeritoNoResponsable;

            return idPermiso == idPermisoPeritoNoResponsable;
        }

        public bool EsPeritoResponsable(int idPermiso)
        {
            int idPermisoPeritoResponsable = (int)TipoPermiso.PeritoResponsable;

            return idPermiso == idPermisoPeritoResponsable;
        }
    }
}
