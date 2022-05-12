using System.Linq;
using Api.Entities;

namespace Api.Repository
{
    public interface ISubcategoriaRepository
    {
        void Cadastrar(SubcategoriaEntity subcategoriaEntity);
        bool Salvar();
        bool ExisteId(int id);
        bool ExisteSubcategoria(SubcategoriaEntity subcategoriaEntity);
        void Alterar(SubcategoriaEntity subcategoriaEntity);
        SubcategoriaEntity Buscar(int id_categoria);
        IQueryable<SubcategoriaEntity> Listar();
        bool Excluir(SubcategoriaEntity subcategoriaEntity);
    }
}