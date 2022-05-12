using System.Collections.Generic;
using System.Linq;
using System;
using Api.Entities;
using Api.Models;
using Api.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class BalancoService : IBalancoService
    {
        private readonly IMapper _mapper;
        private readonly IBalancoRepository _repository;
        private readonly ICategoriaRepository _categoriarepository;

        public BalancoService(IBalancoRepository repository
                             , ICategoriaRepository categoriarepository
                             , IMapper mapper)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            _categoriarepository = categoriarepository ??
                throw new ArgumentNullException(nameof(categoriarepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public BalancoModel Calcular(ParametrosConsultaModel parametrosConsulta)
        {
            var resultado = _repository.Calcular(parametrosConsulta);

            var retornoReceita = CalcularReceita(resultado);
            var retornodespesa = CalcularDespesa(resultado);

            var balanco = new BalancoEntity
            {
                Receita = retornoReceita,
                Despesa = retornodespesa,
                Saldo = retornoReceita + retornodespesa
            };

            if (parametrosConsulta.IdCategoria > 0)
                balanco.Categoria = _categoriarepository.Buscar(parametrosConsulta.IdCategoria);

            return _mapper.Map<BalancoModel>(balanco);
        }

        private Double CalcularReceita(List<LancamentoEntity> lancamentos)
        {
            var valores = lancamentos.Where(x => x.Valor > 0).Select(x => x.Valor).ToList();
            if (valores.Count != 0)
                return valores.Aggregate((acc, x) => acc + x);
            else
                return 0.0f;
        }

        private Double CalcularDespesa(List<LancamentoEntity> lancamentos)
        {
            var valores = lancamentos.Where(x => x.Valor < 0).Select(x => x.Valor).ToList();
            if (valores.Count != 0)
                return valores.Aggregate((acc, x) => acc + x);
            else
                return 0.0f;
        }
    }
}