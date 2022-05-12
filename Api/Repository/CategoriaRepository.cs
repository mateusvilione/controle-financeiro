using System;
using System.Linq;
using Api.DBContext;
using Api.Entities;

namespace Api.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ControleFinanceiroContext _context;

        public CategoriaRepository(ControleFinanceiroContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public CategoriaEntity Buscar(int id_categoria)
        {
            return _context.CategoriaEntities.Where(x => x.Id == id_categoria).FirstOrDefault();
        }

        public void Cadastrar(CategoriaEntity categoriaEntity)
        {
            _context.CategoriaEntities.Add(categoriaEntity);
        }

        public bool ExisteNome(CategoriaEntity categoriaEntity)
        {
            return _context.CategoriaEntities.Any(x => x.Nome == categoriaEntity.Nome && x.Id != categoriaEntity.Id);
        }

        public bool ExisteId(int id)
        {
            return _context.CategoriaEntities.Any(x => x.Id == id);
        }

        public bool Salvar()
        {
            if (_context.SaveChanges() >= 0)
                return true;
            return false;
        }

        public void Alterar(CategoriaEntity categoriaEntity)
        {
            var categoriaOld = _context.CategoriaEntities.Where(x => x.Id == categoriaEntity.Id).FirstOrDefault();

            _context.CategoriaEntities.Update(categoriaOld).CurrentValues.SetValues(categoriaEntity);
        }

        public IQueryable<CategoriaEntity> Listar()
        {
            return _context.CategoriaEntities.AsQueryable();
        }

        public void Excluir(CategoriaEntity categoriaEntity)
        {
            var categoria = _context.CategoriaEntities.Where(x => x.Id == categoriaEntity.Id).First();

            _context.CategoriaEntities.Remove(categoria);
        }
    }
}