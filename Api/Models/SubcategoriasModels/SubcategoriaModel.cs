namespace Api.Models
{
    public class SubcategoriaModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public int id_categoria { get; set; }
    }
}