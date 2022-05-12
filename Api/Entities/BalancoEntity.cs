namespace Api.Entities
{
    public class BalancoEntity
    {
        public double Receita { get; set; }
        public double Despesa { get; set; }
        public double Saldo { get; set; }
        public CategoriaEntity Categoria { get; set; }
    }
}