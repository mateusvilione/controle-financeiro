using System.Linq;
using Api.Entities;

namespace Api.Repository
{
    public interface ILancamentoRepository
    {
        void Cadastrar(LancamentoEntity lancamentoEntity);
        bool Salvar();
        bool ExisteId(int id);
        bool ExisteSubcategoriaId(int id_subcategoria);
        void Alterar(LancamentoEntity lancamentoEntity);
        LancamentoEntity Buscar(int id);
        IQueryable<LancamentoEntity> Listar();
        void Excluir(LancamentoEntity lancamentoEntity);
    }
}