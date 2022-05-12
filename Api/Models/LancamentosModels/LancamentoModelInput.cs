using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class LancamentoModelInput
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
        public double Valor { get; set; }

        public DateTime Data;

        [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
        public int id_subcategoria { get; set; }
        public string Comentario { get; set; }
    }
}