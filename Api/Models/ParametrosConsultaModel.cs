using System;
namespace Api.Models
{
    public class ParametrosConsultaModel
    {
        public int Id { get; set; }
        public int IdSubcategoria { get; set; }
        public int IdCategoria { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Nome { get; set; }
        public string Crescente { get; set; }
        public string Decrescente { get; set; }
        public int Inicio { get; set; }
        public int Limite { get; set; }
    }
}
