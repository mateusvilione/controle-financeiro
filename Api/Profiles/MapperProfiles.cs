using AutoMapper;

namespace Api.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Models.CategoriaModelInput, Entities.CategoriaEntity>();

            CreateMap<Entities.CategoriaEntity, Models.CategoriaModel>();

            CreateMap<Entities.ListaPaginada<Entities.CategoriaEntity>, Models.CategoriaMetadadoModel>()
                .ForPath(
                    destino => destino.metadado.Paginacao.Limite,
                    src => src.MapFrom(src => src.Limite))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAnterior,
                    src => src.MapFrom(src => src.PaginaAnterior))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAtual,
                    src => src.MapFrom(src => src.PaginaAtual))
                .ForPath(
                    destino => destino.metadado.Paginacao.ProximaPagina,
                    src => src.MapFrom(src => src.ProximaPagina))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalItens,
                    src => src.MapFrom(src => src.TotalItens))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalPaginas,
                    src => src.MapFrom(src => src.TotalPaginas))
                .ForPath(
                    destino => destino.resultado,
                    src => src.MapFrom(src => src.Itens));


            CreateMap<Models.SubcategoriaModelInput, Entities.SubcategoriaEntity>();

            CreateMap<Entities.SubcategoriaEntity, Models.SubcategoriaModel>();

            CreateMap<Entities.ListaPaginada<Entities.SubcategoriaEntity>, Models.SubcategoriaMetadadoModel>()
                .ForPath(
                    destino => destino.metadado.Paginacao.Limite,
                    src => src.MapFrom(src => src.Limite))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAnterior,
                    src => src.MapFrom(src => src.PaginaAnterior))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAtual,
                    src => src.MapFrom(src => src.PaginaAtual))
                .ForPath(
                    destino => destino.metadado.Paginacao.ProximaPagina,
                    src => src.MapFrom(src => src.ProximaPagina))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalItens,
                    src => src.MapFrom(src => src.TotalItens))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalPaginas,
                    src => src.MapFrom(src => src.TotalPaginas))
                .ForPath(
                    destino => destino.resultado,
                    src => src.MapFrom(src => src.Itens));


            CreateMap<Models.LancamentoModelInput, Entities.LancamentoEntity>();
            
            CreateMap<Entities.LancamentoEntity, Models.LancamentoModel>();

            CreateMap<Entities.ListaPaginada<Entities.LancamentoEntity>, Models.LancamentoMetadadoModel>()
                .ForPath(
                    destino => destino.metadado.Paginacao.Limite,
                    src => src.MapFrom(src => src.Limite))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAnterior,
                    src => src.MapFrom(src => src.PaginaAnterior))
                .ForPath(
                    destino => destino.metadado.Paginacao.PaginaAtual,
                    src => src.MapFrom(src => src.PaginaAtual))
                .ForPath(
                    destino => destino.metadado.Paginacao.ProximaPagina,
                    src => src.MapFrom(src => src.ProximaPagina))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalItens,
                    src => src.MapFrom(src => src.TotalItens))
                .ForPath(
                    destino => destino.metadado.Paginacao.TotalPaginas,
                    src => src.MapFrom(src => src.TotalPaginas))
                .ForPath(
                    destino => destino.resultado,
                    src => src.MapFrom(src => src.Itens));

            CreateMap<Entities.BalancoEntity, Models.BalancoModel>();
        }
    }
}