using System;
using System.Linq;
using Api.DBContext;
using Api.Entities;

namespace Api.Repository
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly ControleFinanceiroContext _context;

        public LancamentoRepository(ControleFinanceiroContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public LancamentoEntity Buscar(int id)
        {
            return _context.LancamentoEntities.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Cadastrar(LancamentoEntity lancamentoEntity)
        {
            _context.LancamentoEntities.Add(lancamentoEntity);
        }

        public bool ExisteSubcategoriaId(int id_subcategoria)
        {
            return _context.SubcategoriaEntities.Any(x => x.Id == id_subcategoria);
        }

        public bool ExisteId(int id)
        {
            return _context.LancamentoEntities.Any(x => x.Id == id);
        }

        public bool Salvar()
        {
            if (_context.SaveChanges() >= 0)
                return true;
            return false;
        }

        public void Alterar(LancamentoEntity lancamentoEntity)
        {
            var lancamentoOld = _context.LancamentoEntities.Where(x => x.Id == lancamentoEntity.Id).FirstOrDefault();

            _context.LancamentoEntities.Update(lancamentoOld).CurrentValues.SetValues(lancamentoEntity);
        }

        public IQueryable<LancamentoEntity> Listar()
        {
            return _context.LancamentoEntities.AsQueryable();
        }

        public void Excluir(LancamentoEntity lancamentoEntity)
        {
            var lancamento = _context.LancamentoEntities.Where(x => x.Id == lancamentoEntity.Id).First();

            _context.LancamentoEntities.Remove(lancamento);
        }
    }
}