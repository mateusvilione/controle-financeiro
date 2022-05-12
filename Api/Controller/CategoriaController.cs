using System;
using System.Linq;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controller
{
    [ApiController]
    [Route("/v1/categorias")]
    [EnableCors("AllowOrigin")]
    public class CategoriaController : ControllerBase
    {
        private const string MSG_ERRO_500 = "Ocorreu um erro inesperado";
        private readonly ILogger<CategoriaController> _logger;
        private readonly ICategoriaService _service;
        private readonly IMapper _mapper;

        public CategoriaController(ILogger<CategoriaController> logger
                                  , ICategoriaService service
                                  , IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _service = service ??
                throw new ArgumentNullException(nameof(service));

            _mapper = mapper ??
                    throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Cadastrar(CategoriaModelInput categoriaRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                var categoriaEntity = _mapper.Map<Entities.CategoriaEntity>(categoriaRequest);
                _service.Cadastrar(categoriaEntity);

                return CreatedAtAction(null, null);
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(new ErroModel("erro_validacao", ex.Message));
            }
            catch (Exception erro)
            {
                _logger.LogError($"Data: {DateTime.Now}, Erro: {erro}, Mensagem:{erro.Message}");
                return new JsonResult(500, new ErroModel("erro_interno", MSG_ERRO_500));
            }
        }

        [HttpPut("{id_categoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Alterar(int id_categoria, CategoriaModelInput categoriaRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                var categoriaEntity = _mapper.Map<Entities.CategoriaEntity>(categoriaRequest);
                categoriaEntity.Id = id_categoria;

                if (_service.ExisteNome(categoriaEntity))
                    return new ConflictObjectResult(new ErroModel("erro_conflito", "O nome já está sendo utilizado"));

                _service.Alterar(categoriaEntity);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(new ErroModel("erro_validacao", ex.Message));
            }
            catch (Exception erro)
            {
                _logger.LogError($"Data: {DateTime.Now}, Erro: {erro}, Mensagem:{erro.Message}");
                return new JsonResult(500, new ErroModel("erro_interno", MSG_ERRO_500));
            }
        }

        [HttpGet("{id_categoria}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Buscar(int id_categoria)
        {
            try
            {
                var retorno = _service.Buscar(id_categoria);

                if (retorno != null)
                    return Ok(_mapper.Map<CategoriaModel>(retorno));
                else
                    return NoContent();
            }
            catch (Exception erro)
            {
                _logger.LogError($"Data: {DateTime.Now}, Erro: {erro}, Mensagem:{erro.Message}");
                return new JsonResult(500, new ErroModel("erro_interno", MSG_ERRO_500));
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaMetadadoModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Listar(string nome, string crescente, string decrescente, int? inicio = 1, int? limite = 10)
        {
            try
            {
                var retorno = _service.Listar(new ParametrosConsultaModel
                {
                    Nome = nome,
                    Crescente = crescente,
                    Decrescente = decrescente,
                    Inicio = inicio.GetValueOrDefault(),
                    Limite = limite.GetValueOrDefault(),
                });

                if (retorno != null && retorno.Itens.Count != 0)
                    return Ok(_mapper.Map<CategoriaMetadadoModel>(retorno));
                else
                    return NoContent();
            }
            catch (Exception erro)
            {
                _logger.LogError($"Data: {DateTime.Now}, Erro: {erro}, Mensagem:{erro.Message}");
                return new JsonResult(500, new ErroModel("erro_interno", MSG_ERRO_500));
            }
        }

        [HttpDelete("{id_categoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Deletar(int id_categoria)
        {
            try
            {
                if (!_service.ExisteId(id_categoria))
                    return new NotFoundObjectResult(new ErroModel("erro_validacao", "A categoria que deseja excluir não existe na base de dados"));


                var categoriaDeletado = _service.Buscar(id_categoria);

                _service.Excluir(categoriaDeletado);

                return Ok();
            }
            catch (Exception erro)
            {
                _logger.LogError($"Data: {DateTime.Now}, Erro: {erro}, Mensagem:{erro.Message}");
                return new JsonResult(500, new ErroModel("erro_interno", MSG_ERRO_500));
            }
        }

    }
}