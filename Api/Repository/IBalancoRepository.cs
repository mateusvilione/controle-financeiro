using System.Collections.Generic;
using Api.Entities;
using Api.Models;

namespace Api.Repository
{
    public interface IBalancoRepository
    {
        List<LancamentoEntity> Calcular(ParametrosConsultaModel parametrosConsulta);
    }
}