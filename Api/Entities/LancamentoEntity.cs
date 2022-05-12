using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    [Table("lancamento")]
    public class LancamentoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public double Valor { get; set; }
        public DateTime Data { get; set; }

        [Required]
        public int id_subcategoria { get; set; }

        [ForeignKey("id_subcategoria")]
        public SubcategoriaEntity Subcategoria { get; set; }

        public string Comentario { get; set; }
    }
}