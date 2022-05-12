using System;
using System.Linq;
using Api.DBContext;
using Api.Entities;

namespace Api.Repository
{
    public class SubcategoriaRepository : ISubcategoriaRepository
    {
        private readonly ControleFinanceiroContext _context;

        public SubcategoriaRepository(ControleFinanceiroContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public SubcategoriaEntity Buscar(int id_categoria)
        {
            return _context.SubcategoriaEntities.Where(x => x.Id == id_categoria).FirstOrDefault();
        }

        public void Cadastrar(SubcategoriaEntity subcategoriaEntity)
        {
            _context.SubcategoriaEntities.Add(subcategoriaEntity);
        }

        public bool ExisteSubcategoria(SubcategoriaEntity subcategoriaEntity)
        {
            return _context.SubcategoriaEntities.Any(x => x.Nome == subcategoriaEntity.Nome && x.id_categoria == subcategoriaEntity.id_categoria && x.Id != subcategoriaEntity.Id);
        }

        public bool ExisteId(int id)
        {
            return _context.SubcategoriaEntities.Any(x => x.Id == id);
        }

        public bool Salvar()
        {
            if (_context.SaveChanges() >= 0)
                return true;
            return false;
        }

        public void Alterar(SubcategoriaEntity subcategoriaEntity)
        {
            var subcategoriaOld = _context.SubcategoriaEntities.Where(x => x.Id == subcategoriaEntity.Id).FirstOrDefault();

            _context.SubcategoriaEntities.Update(subcategoriaOld).CurrentValues.SetValues(subcategoriaEntity);
        }

        public IQueryable<SubcategoriaEntity> Listar()
        {
            return _context.SubcategoriaEntities.AsQueryable();
        }

        public bool Excluir(SubcategoriaEntity subcategoriaEntity)
        {
            var lancamentos = _context.LancamentoEntities.Any(x => x.id_subcategoria == subcategoriaEntity.Id);
            if (!lancamentos)
            {
                var subcategoria = _context.SubcategoriaEntities.Where(x => x.Id == subcategoriaEntity.Id).First();
                _context.SubcategoriaEntities.Remove(subcategoria);
                return true;
            }
            return false;
        }
    }
}