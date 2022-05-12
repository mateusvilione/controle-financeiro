using System.Linq;
using System;
using Api.Entities;
using Api.Models;
using Api.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class SubcategoriaService : ISubcategoriaService
    {

        private readonly IPaginarService<SubcategoriaEntity> _paginarService;
        private readonly IMapper _mapper;
        private readonly ISubcategoriaRepository _repository;

        public SubcategoriaService(IPaginarService<SubcategoriaEntity> paginarService
                                  , ISubcategoriaRepository repository
                                  , IMapper mapper)
        {
            _paginarService = paginarService ??
                throw new ArgumentNullException(nameof(paginarService));

            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void Cadastrar(SubcategoriaEntity subcategoriaEntity)
        {
            if (subcategoriaEntity is null)
                throw new ArgumentNullException(nameof(subcategoriaEntity));

            if (_repository.ExisteSubcategoria(subcategoriaEntity))
                throw new ArgumentException("Subcategoria já cadastrada");

            _repository.Cadastrar(subcategoriaEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public void Alterar(SubcategoriaEntity subcategoriaEntity)
        {
            if (subcategoriaEntity is null)
                throw new ArgumentNullException(nameof(subcategoriaEntity));

            _repository.Alterar(subcategoriaEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public bool ExisteNome(SubcategoriaEntity subcategoriaEntity)
        {
            if (subcategoriaEntity is null)
                throw new ArgumentNullException(nameof(subcategoriaEntity));

            return _repository.ExisteSubcategoria(subcategoriaEntity);
        }

        public SubcategoriaEntity Buscar(int id_subcategoria)
        {
            return _repository.Buscar(id_subcategoria);
        }

        public ListaPaginada<SubcategoriaEntity> Listar(ParametrosConsultaModel parametrosConsulta)
        {
            var subcategorias = _repository.Listar().OrderBy(x => x.Id).AsQueryable();

            if (parametrosConsulta.Id > 0)
                subcategorias = subcategorias.Where(x => x.Id == parametrosConsulta.Id).AsQueryable();

            if (!string.IsNullOrEmpty(parametrosConsulta.Nome))
                subcategorias = subcategorias.Where(x => x.Nome.Contains(parametrosConsulta.Nome)).AsQueryable();


            if (parametrosConsulta.Crescente != null)
            {
                switch (parametrosConsulta.Crescente.ToUpper())
                {
                    case "NOME":
                        subcategorias = subcategorias.OrderBy(x => x.Nome).AsQueryable();
                        break;
                }
            }
            else if (parametrosConsulta.Decrescente != null)
            {
                switch (parametrosConsulta.Decrescente.ToUpper())
                {
                    case "NOME":
                        subcategorias = subcategorias.OrderByDescending(x => x.Nome).AsQueryable();
                        break;
                }
            }
            return _paginarService.Paginar(subcategorias.AsNoTracking(), parametrosConsulta.Inicio, parametrosConsulta.Limite);
        }

        public void Excluir(SubcategoriaEntity subcategoriaEntity)
        {
            if (subcategoriaEntity is null)
                throw new ArgumentNullException(nameof(subcategoriaEntity));

            var retorno = _repository.Excluir(subcategoriaEntity);

            if (!retorno)
                throw new ArgumentException("Há lancamentos cadastrados");
            
            if (!_repository.Salvar())
                throw new Exception("Não foi possível Excluir");
        }

        public bool ExisteId(int id)
        {
            return _repository.ExisteId(id);
        }
    }
}