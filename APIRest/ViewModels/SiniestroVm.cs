﻿using APIRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class SiniestroVm
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdAseguradora { get; set; }
        public string Aseguradora { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public int IdPerito { get; set; }
        public string Perito { get; set; }
        public string FechaHoraAlta { get; set; }
        public int IdSujetoAfectado { get; set; }
        public string SujetoAfectado { get; set; }
        public int IdDanio { get; set; }
        public string Danio { get; set; }
        public decimal ImpValoracionDanios { get; set; }
        public int IdUsuarioAlta { get; set; }        

        public static List<SiniestroVm> ConvertirASiniestroVm(List<Siniestro> siniestros)
        {
            List<SiniestroVm> siniestrosVms = siniestros.Select(siniestro => new SiniestroVm()
            {
                Id = siniestro.Id,
                IdEstado = siniestro.Estado.Id,
                Estado = siniestro.Estado.Nombre,
                Aseguradora = siniestro.Aseguradora.Nombre,
                IdAseguradora = siniestro.Aseguradora.Id,
                Descripcion = siniestro.Descripcion,
                Perito = siniestro.Perito.Nombre,
                IdPerito = siniestro.Perito.Id,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = siniestro.ImpValoracionDanios
            })
            .ToList();

            return siniestrosVms;
        }
    }
}
