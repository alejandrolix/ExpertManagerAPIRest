using APIRest.Context;
using APIRest.Excepciones;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
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

        public async Task<TokenUsuario> ObtenerDatosToken(string token)
        {
            TokenUsuario tokenUsuario = await _contexto.TokensUsuario
                                                       .Where(t => t.Token == token)
                                                       .FirstOrDefaultAsync();
            if (tokenUsuario is null)
                throw new CodigoErrorHttpException($"No existe el token {token}", HttpStatusCode.NotFound);

            return tokenUsuario;
        }
    }
}
