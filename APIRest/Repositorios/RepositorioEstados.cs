using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioEstados
    {
        public enum Tipo
        {
            Procesando = 1,
            SinValorar = 2,
            Valorado = 3,
            Cerrado = 4
        }
    }
}
