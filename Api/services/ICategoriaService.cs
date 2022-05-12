using System.Collections.Generic;
using Api.Entities;
using Api.Models;

namespace Api.Services
{
    public interface ICategoriaService
    {
        void Cadastrar(CategoriaEntity categoriaEntity);
        void Alterar(CategoriaEntity categoriaEntity);
        ListaPaginada<CategoriaEntity> Listar(ParametrosConsultaModel parametrosConsulta);
        bool ExisteNome(CategoriaEntity categoriaEntity);
        bool ExisteId(int id);
        CategoriaEntity Buscar(int id_categoria);
        void Excluir(CategoriaEntity categoriaEntity);
    }
}