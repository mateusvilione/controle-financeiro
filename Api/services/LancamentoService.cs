using System.Linq;
using System;
using Api.Entities;
using Api.Models;
using Api.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class LancamentoService : ILancamentoService
    {

        private readonly IPaginarService<LancamentoEntity> _paginarService;
        private readonly ILancamentoRepository _repository;
        private readonly ISubcategoriaRepository _subcategoriarepository;

        public LancamentoService(IPaginarService<LancamentoEntity> paginarService
                                , ILancamentoRepository repository
                                , ISubcategoriaRepository subcategoriarepository)
        {
            _paginarService = paginarService ??
                throw new ArgumentNullException(nameof(paginarService));

            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            _subcategoriarepository = subcategoriarepository ??
                throw new ArgumentNullException(nameof(subcategoriarepository));
        }

        public void Cadastrar(LancamentoEntity lancamentoEntity)
        {
            if (lancamentoEntity is null)
                throw new ArgumentNullException(nameof(lancamentoEntity));

            if (!_subcategoriarepository.ExisteId(lancamentoEntity.id_subcategoria))
                throw new ArgumentException("subcategoria não cadastrada");

            _repository.Cadastrar(lancamentoEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public void Alterar(LancamentoEntity lancamentoEntity)
        {
            if (lancamentoEntity is null)
                throw new ArgumentNullException(nameof(lancamentoEntity));

            if (!_subcategoriarepository.ExisteId(lancamentoEntity.id_subcategoria))
                throw new ArgumentException("subcategoria não cadastrada");

            _repository.Alterar(lancamentoEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível salvar");
        }

        public LancamentoEntity Buscar(int id)
        {
            return _repository.Buscar(id);
        }

        public ListaPaginada<LancamentoEntity> Listar(ParametrosConsultaModel parametrosConsulta)
        {
            var lancamentos = _repository.Listar().OrderBy(x => x.Id).AsQueryable();

            if (parametrosConsulta.IdSubcategoria > 0)
                lancamentos = lancamentos.Where(x => x.id_subcategoria == parametrosConsulta.IdSubcategoria).AsQueryable();

            if (parametrosConsulta.DataInicio != DateTime.Now)
                lancamentos = lancamentos.Where(x => x.Data >= parametrosConsulta.DataInicio).AsQueryable();

            if (parametrosConsulta.DataFim != DateTime.Now)
                lancamentos = lancamentos.Where(x => x.Data <= parametrosConsulta.DataFim.AddHours(23).AddMinutes(59).AddSeconds(59)).AsQueryable();

            if (parametrosConsulta.Crescente != null)
            {
                switch (parametrosConsulta.Crescente.ToUpper())
                {
                    case "DATA":
                        lancamentos = lancamentos.OrderBy(x => x.Data).AsQueryable();
                        break;
                }
            }
            else if (parametrosConsulta.Decrescente != null)
            {
                switch (parametrosConsulta.Decrescente.ToUpper())
                {
                    case "DATA":
                        lancamentos = lancamentos.OrderByDescending(x => x.Data).AsQueryable();
                        break;
                }
            }
            return _paginarService.Paginar(lancamentos.AsNoTracking(), parametrosConsulta.Inicio, parametrosConsulta.Limite);
        }

        public void Excluir(LancamentoEntity lancamentoEntity)
        {
            if (lancamentoEntity is null)
                throw new ArgumentNullException(nameof(lancamentoEntity));

            _repository.Excluir(lancamentoEntity);

            if (!_repository.Salvar())
                throw new Exception("Não foi possível Excluir");
        }

        public bool ExisteId(int id)
        {
            return _repository.ExisteId(id);
        }
    }
}