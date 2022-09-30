using System;
using System.ComponentModel.DataAnnotations;

namespace APIRest.Models
{
    public class TokenUsuario
    {
        [Key]
        public string Token { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
