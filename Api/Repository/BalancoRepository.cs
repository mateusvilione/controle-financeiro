using System;
using System.Collections.Generic;
using System.Linq;
using Api.DBContext;
using Api.Entities;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class BalancoRepository : IBalancoRepository
    {
        private readonly ControleFinanceiroContext _context;

        public BalancoRepository(ControleFinanceiroContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public List<LancamentoEntity> Calcular(ParametrosConsultaModel parametrosConsulta)
        {

            var resultado = from x in _context.LancamentoEntities.Include("Subcategoria")
                            where x.Data >= parametrosConsulta.DataInicio && x.Data <= parametrosConsulta.DataFim.AddHours(23).AddMinutes(59).AddSeconds(59)
                            select x;

            if (parametrosConsulta.IdCategoria > 0)
                resultado = resultado.Where(x => x.Subcategoria.id_categoria == parametrosConsulta.IdCategoria).AsQueryable();

            return resultado.ToList();
        }

    }
}