using System.Linq;
using System;
using Api.Entities;
using Api.Models;
using Api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class CategoriaService : ICategoriaService
    {

        private readonly IPaginarService<CategoriaEntity> _paginarService;
        private readonly ICategoriaRepository _repository;

        public CategoriaService(IPaginarService<CategoriaEntity> paginarService
                               , ICategoriaRepository repository)
        {
            _paginarService = paginarService ??
                throw new ArgumentNullException(nameof(paginarService));

            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public void Cadastrar(CategoriaEntity categoriaEntity)
        {
            if (categoriaEntity is null)
                throw new ArgumentNullException(nameof(categoriaEntity));

            if (_repository.ExisteNome(categoriaEntity))
                throw new ArgumentException("Categoria já cadastrada");

            _repository.Cadastrar(categoriaEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public void Alterar(CategoriaEntity categoriaEntity)
        {
            if (categoriaEntity is null)
                throw new ArgumentNullException(nameof(categoriaEntity));

            _repository.Alterar(categoriaEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public bool ExisteNome(CategoriaEntity categoriaEntity)
        {
            if (categoriaEntity is null)
                throw new ArgumentNullException(nameof(categoriaEntity));

            return _repository.ExisteNome(categoriaEntity);
        }

        public CategoriaEntity Buscar(int id_categoria)
        {
            return _repository.Buscar(id_categoria);
        }

        public ListaPaginada<CategoriaEntity> Listar(ParametrosConsultaModel parametrosConsulta)
        {
            var categorias = _repository.Listar().OrderBy(x => x.Id).AsQueryable();

            if (!string.IsNullOrEmpty(parametrosConsulta.Nome))
                categorias = categorias.Where(x => x.Nome.Contains(parametrosConsulta.Nome)).AsQueryable();

            if (parametrosConsulta.Crescente != null)
            {
                switch (parametrosConsulta.Crescente.ToUpper())
                {
                    case "NOME":
                        categorias = categorias.OrderBy(x => x.Nome).AsQueryable();
                        break;
                }
            }
            else if (parametrosConsulta.Decrescente != null)
            {
                switch (parametrosConsulta.Decrescente.ToUpper())
                {
                    case "NOME":
                        categorias = categorias.OrderByDescending(x => x.Nome).AsQueryable();
                        break;
                }
            }
            return _paginarService.Paginar(categorias.AsNoTracking(), parametrosConsulta.Inicio, parametrosConsulta.Limite);
        }

        public void Excluir(CategoriaEntity categoriaEntity)
        {
            if (categoriaEntity is null)
                throw new ArgumentNullException(nameof(categoriaEntity));

            _repository.Excluir(categoriaEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível Excluir");
        }

        public bool ExisteId(int id)
        {
            return _repository.ExisteId(id);
        }
    }
}