using System.Linq;
using Api.Entities;

namespace Api.Services
{
    public interface IPaginarService<T>
    {
        ListaPaginada<T> Paginar(IQueryable<T> itens, int inicio, int limite);
    }
}
