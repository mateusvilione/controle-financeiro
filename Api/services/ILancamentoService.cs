using Api.Entities;
using Api.Models;

namespace Api.Services
{
    public interface ILancamentoService
    {
        void Cadastrar(LancamentoEntity lancamentoEntity);
        void Alterar(LancamentoEntity lancamentoEntity);
        LancamentoEntity Buscar(int id);
        ListaPaginada<LancamentoEntity> Listar(ParametrosConsultaModel parametrosConsulta);
        void Excluir(LancamentoEntity lancamentoEntity);
        bool ExisteId(int id);
    }
}