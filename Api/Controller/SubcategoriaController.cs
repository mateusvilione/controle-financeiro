using System;
using System.Linq;
using Api.Entities;
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
    [Route("/v1/subcategorias")]
    [EnableCors("AllowOrigin")]
    public class SubcategoriaController : ControllerBase
    {
        private const string MSG_ERRO_500 = "Ocorreu um erro inesperado";
        private readonly ILogger<SubcategoriaController> _logger;
        private readonly ISubcategoriaService _service;
        private readonly IMapper _mapper;

        public SubcategoriaController(ILogger<SubcategoriaController> logger
                                     , ISubcategoriaService service
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
        public IActionResult Cadastrar(SubcategoriaModelInput subcategoriaRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                var subcategoriaEntity = _mapper.Map<Entities.SubcategoriaEntity>(subcategoriaRequest);
                _service.Cadastrar(subcategoriaEntity);

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

        [HttpPut("{id_subcategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Alterar(int id_subcategoria, SubcategoriaModelInput subcategoriaRequest)
        {
            try
            {
                var subcategoriaEntity = _mapper.Map<Entities.SubcategoriaEntity>(subcategoriaRequest);
                subcategoriaEntity.Id = id_subcategoria;

                if (_service.ExisteNome(subcategoriaEntity))
                    return new ConflictObjectResult(new ErroModel("erro_conflito", "O nome já está sendo utilizado"));

                _service.Alterar(subcategoriaEntity);

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

        [HttpGet("{id_subcategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Buscar(int id_subcategoria)
        {
            try
            {
                var retorno = _service.Buscar(id_subcategoria);

                if (retorno != null)
                    return Ok(_mapper.Map<SubcategoriaModel>(retorno));
                else
                    return NoContent();
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


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LancamentoMetadadoModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Listar(int id, string nome, string crescente, string decrescente, int? inicio = 1, int? limite = 10)
        {
            try
            {
                ListaPaginada<SubcategoriaEntity> retorno = _service.Listar(new ParametrosConsultaModel
                {
                    Id = id,
                    Nome = nome,
                    Crescente = crescente,
                    Decrescente = decrescente,
                    Inicio = inicio.GetValueOrDefault(),
                    Limite = limite.GetValueOrDefault(),
                });

                if (retorno != null && retorno.Itens.Count != 0)
                    return Ok(_mapper.Map<SubcategoriaMetadadoModel>(retorno));
                else
                    return NoContent();
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

        [HttpDelete("{id_subcategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Deletar(int id_subcategoria)
        {
            try
            {
                if (!_service.ExisteId(id_subcategoria))
                    return new NotFoundObjectResult(new ErroModel("erro_validacao", "A subcategoria que deseja excluir não existe na base de dados"));


                var categoriaDeletado = _service.Buscar(id_subcategoria);

                _service.Excluir(categoriaDeletado);

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

    }
}