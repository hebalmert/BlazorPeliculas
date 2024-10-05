namespace BlazorPeliculas.Shared.Entidades
{
    public class SelectorMultipleModel
    {
        public SelectorMultipleModel( string llave, string valor)
        {
            Llave = llave;
            Valor = valor;
        }

        public string Llave { get; set; }

        public string Valor { get; set; }
    }
}
