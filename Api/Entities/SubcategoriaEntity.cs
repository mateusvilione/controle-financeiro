using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    [Table("subcategoria")]
    public class SubcategoriaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int id_categoria { get; set; }

        [ForeignKey("id_categoria")]
        public CategoriaEntity Categoria { get; set; }
    }
}