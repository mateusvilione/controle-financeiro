using System;
using System.Linq;
using Api.Entities;

namespace Api.Services
{

    public class PaginarService<T> : IPaginarService<T>
    {
        public ListaPaginada<T> Paginar(IQueryable<T> itens, int inicio, int limite)
        {
            if (inicio <= 0)
            {
                throw new ArgumentException("A pagina deve ser maior que 0");
            }

            if (limite < 5 || limite > 30)
            {
                throw new ArgumentException("O limite deve ser preenchido com valores entre 5 e 30");
            }

            var totalItens = itens.Count();
            var itensPaginados = itens.Skip((inicio - 1) * limite).Take(limite).ToList();
            return new ListaPaginada<T>(itensPaginados, totalItens, inicio, limite);
        }
    }
}