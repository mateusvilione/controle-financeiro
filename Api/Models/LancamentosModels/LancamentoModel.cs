using System;
namespace Api.Models
{
    public class LancamentoModel
    {
        public int Id { get; set; }

        public double Valor { get; set; }
        public DateTime Data { get; set; }
        public int id_subcategoria { get; set; }
        public string Comentario { get; set; }
    }
}