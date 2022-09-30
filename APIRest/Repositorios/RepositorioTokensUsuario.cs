using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioTokensUsuario
    {
        private ExpertManagerContext _contexto;

        public RepositorioTokensUsuario(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task GuardarToken(string token, Usuario usuario)
        {
            TokenUsuario tokenUsuario = new TokenUsuario
            {
                Token = token,
                Usuario = usuario,
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now.AddMinutes(30)
            };

            try
            {
                _contexto.Add(tokenUsuario);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
