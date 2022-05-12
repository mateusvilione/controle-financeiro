using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CategoriaModelInput
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
        public string Nome { get; set; }
    }
}