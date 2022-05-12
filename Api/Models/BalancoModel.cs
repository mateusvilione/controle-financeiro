using System;

namespace Api.Models
{
    public class BalancoModel
    {
        public double Receita { get; set; }
        public double Despesa { get; set; }
        public double Saldo { get; set; }
        public CategoriaModel Categoria { get; set; }
    }
}