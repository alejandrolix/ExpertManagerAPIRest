using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Siniestro
    {
        public int Id { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
        public int AseguradoraId { get; set; }
        public Aseguradora Aseguradora { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int UsuarioCreadoId { get; set; }
        public Usuario UsuarioCreado { get; set; }

        [Required]
        public DateTime FechaHoraAlta { get; set; }

        [Required]
        public SujetoAfectado SujetoAfectado { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ImpValoracionDanios { get; set; }

        public int PeritoId { get; set; }
        public Usuario Perito { get; set; }
        public int? DanioId { get; set; }
        public Danio Danio { get; set; }
    }

    public enum SujetoAfectado
    {
        Asegurado = 0, Contrario = 1
    }
}
