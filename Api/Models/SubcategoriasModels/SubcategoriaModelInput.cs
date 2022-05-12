using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class SubcategoriaModelInput
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
        public int id_categoria { get; set; }
    }
}