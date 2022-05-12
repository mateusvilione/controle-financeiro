using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Api.Entities;
using Api.Services;

namespace Tests.Services
{

    public class PaginarServiceTest
    {

        private readonly IPaginarService<CategoriaEntity> service;


        public PaginarServiceTest()
        {
            service = new PaginarService<CategoriaEntity>();
        }

        [Theory]
        [InlineData(1, 10, 100)]
        [InlineData(1, 20, 100)]
        [InlineData(10, 10, 100)]
        [InlineData(2, 30, 100)]
        [InlineData(4, 30, 100)]
        public void PagincacaoSucesso(int pagina, int limite, int total)
        {
            List<CategoriaEntity> categoria = GerarConteudo(pagina, limite, total);

            var CategoriaPaginados = service.Paginar(categoria.AsQueryable(), pagina, limite);

            int? paginaAnterior = null;
            int? proximaPagina = null;

            var totalPaginas = (int)Math.Ceiling(categoria.Count / (double)limite);
            var totalItensPagina = categoria.Skip((pagina - 1) * limite).Take(limite).ToList().Count;

            if (pagina < totalPaginas)
                proximaPagina = pagina + 1;

            if (pagina > 1)
                paginaAnterior = pagina - 1;

            Assert.NotNull(CategoriaPaginados);
            Assert.Equal(CategoriaPaginados.PaginaAtual, pagina);
            Assert.Equal(CategoriaPaginados.Itens.Count, totalItensPagina);
            Assert.Equal(CategoriaPaginados.TotalPaginas, totalPaginas);
            Assert.Equal(CategoriaPaginados.TotalItens, categoria.Count);
            Assert.Equal(CategoriaPaginados.Limite, limite);
            Assert.Equal(CategoriaPaginados.PaginaAnterior, paginaAnterior);
            Assert.Equal(CategoriaPaginados.ProximaPagina, proximaPagina);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 5)]
        [InlineData(1, 31)]
        [InlineData(1, 4)]
        public void PaginacaoErro(int pagina, int limite)
        {
            Assert.Throws<ArgumentException>(() => service.Paginar(null, pagina, limite));
        }

        private List<CategoriaEntity> GerarConteudo(int pagina, int limite, int total)
        {
            List<CategoriaEntity> categoria = new List<CategoriaEntity>();

            for (int i = 0; i < total; i++)
                categoria.Add(new CategoriaEntity()
                {
                    Id = 1,
                    Nome = ""
                });

            return categoria;
        }



    }


}
