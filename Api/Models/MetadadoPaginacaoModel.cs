namespace Api.Models
{
    public class MetadadoPaginacaoModel
    {
        public int Limite { get; set; }

        public int? PaginaAnterior { get; set; }

        public int? ProximaPagina { get; set; }

        public int? PaginaAtual { get; set; }

        public int? TotalPaginas { get; set; }

        public int TotalItens { get; set; }
    }
}