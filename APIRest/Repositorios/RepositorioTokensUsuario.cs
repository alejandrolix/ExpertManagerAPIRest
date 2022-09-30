using APIRest.Context;

namespace APIRest.Repositorios
{
    public class RepositorioTokensUsuario
    {
        private ExpertManagerContext _contexto;

        public RepositorioTokensUsuario(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }
    }
}
