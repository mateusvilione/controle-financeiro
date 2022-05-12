using System.Linq;
using Api.Entities;

namespace Api.Repository
{
    public interface ICategoriaRepository
    {
        void Cadastrar(CategoriaEntity categoriaEntity);
        bool Salvar();
        bool ExisteId(int id);
        bool ExisteNome(CategoriaEntity categoriaEntity);
        void Alterar(CategoriaEntity categoriaEntity);
        CategoriaEntity Buscar(int id_categoria);
        IQueryable<CategoriaEntity> Listar();
        void Excluir(CategoriaEntity categoriaEntity);
    }
}