using System;
using System.Collections.Generic;

namespace Api.Entities
{
    public class ListaPaginada<T> : List<T>
    {
        public ListaPaginada(List<T> itens, int totalItens, int paginaAtual, int limitePagina)
        {
            PaginaAtual = paginaAtual;
            TotalItens = totalItens;
            Limite = limitePagina;
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)limitePagina);
            Itens = new List<T>();
            Itens.AddRange(itens);
        }

        public int Limite { get; }

        public int PaginaAtual { get; }

        public int TotalPaginas { get; }

        public int TotalItens { get; }

        public List<T> Itens { get; }
        public int? PaginaAnterior
        {
            get
            {
                if (PaginaAtual > 1)
                    return PaginaAtual - 1;

                return null;

            }
        }

        public int? ProximaPagina
        {
            get
            {
                if (TemProximaPagina)
                    return PaginaAtual + 1;

                return null;

            }
        }

        public bool TemProximaPagina
        {
            get
            {
                return (PaginaAtual < TotalPaginas);
            }
        }
    }
}