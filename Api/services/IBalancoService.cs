using Api.Models;

namespace Api.Services
{
    public interface IBalancoService
    {
        BalancoModel Calcular(ParametrosConsultaModel parametrosConsulta);
    }
}