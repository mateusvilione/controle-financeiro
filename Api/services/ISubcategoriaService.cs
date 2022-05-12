using Api.Entities;
using Api.Models;

namespace Api.Services
{
    public interface ISubcategoriaService
    {
        void Cadastrar(SubcategoriaEntity subcategoriaEntity);
        void Alterar(SubcategoriaEntity subcategoriaEntity);
        ListaPaginada<SubcategoriaEntity> Listar(ParametrosConsultaModel parametrosConsulta);
        bool ExisteNome(SubcategoriaEntity subcategoriaEntity);
        bool ExisteId(int id);
        SubcategoriaEntity Buscar(int id_subcategoria);

        void Excluir(SubcategoriaEntity subcategoriaEntity);
    }
}