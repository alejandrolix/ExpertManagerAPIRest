namespace APIRest.Filtros.ActionFilters
{
    public class RutaExcluida
    {
        public TipoMetodo Metodo { get; set; }
        public string Ruta { get; set; }

        public RutaExcluida(TipoMetodo metodo, string ruta)
        {
            Metodo = metodo;
            Ruta = ruta;
        }
    }

    public enum TipoMetodo
    {
        GET,
        POST
    }
}
