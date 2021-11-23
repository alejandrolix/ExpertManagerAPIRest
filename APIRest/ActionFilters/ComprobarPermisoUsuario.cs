using APIRest.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Repositorios;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIRest.ActionFilters
{
    public class ComprobarPermisoUsuario : ActionFilterAttribute
    {
        private RepositorioUsuarios RepositorioUsuarios;
        private RepositorioPermisos RepositorioPermisos;

        public ComprobarPermisoUsuario(RepositorioUsuarios repositorioUsuarios, RepositorioPermisos repositorioPermisos)
        {
            RepositorioUsuarios = repositorioUsuarios;
            RepositorioPermisos = repositorioPermisos;
        }

        public override async Task<Task> OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int idUsuario = 0;
            bool comprobarPermisoUsuario = false;

            if (context.ActionArguments.ContainsKey("siniestroVm"))
            {
                comprobarPermisoUsuario = true;

                SiniestroVm siniestroVm = (SiniestroVm)context.ActionArguments["siniestroVm"];
                idUsuario = siniestroVm.IdUsuarioAlta;
            }
            else if (context.ActionArguments.ContainsKey("cerrarSiniestroVm"))
            {
                comprobarPermisoUsuario = true;

                CerrarSiniestroVm cerrarSiniestroVm = (CerrarSiniestroVm)context.ActionArguments["cerrarSiniestroVm"];
                idUsuario = cerrarSiniestroVm.IdPerito;
            }

            if (!comprobarPermisoUsuario)
                return base.OnActionExecutionAsync(context, next);

            Usuario usuario = await RepositorioUsuarios.ObtenerPorId(idUsuario);

            if (RepositorioPermisos.TienePermisoAdministracion(usuario.Id))            
                return base.OnActionExecutionAsync(context, next);            

            var error = new
            {
                error = "El usuario no tiene permiso para realizar la acción"
            };

            context.Result = new ObjectResult(error)
            {
                StatusCode = 401
            };

            return base.OnActionExecutionAsync(context, next);
        }
    }
}
