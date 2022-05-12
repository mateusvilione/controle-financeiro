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
    [Route("/v1/lancamentos")]
    [EnableCors("AllowOrigin")]
    public class LancamentoController : ControllerBase
    {
        private const string MSG_ERRO_500 = "Ocorreu um erro inesperado";
        private readonly ILogger<LancamentoController> _logger;
        private readonly ILancamentoService _service;
        private readonly IMapper _mapper;

        public LancamentoController(ILogger<LancamentoController> logger
                                   , ILancamentoService service
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
        public IActionResult Cadastrar(LancamentoModelInput lancamentoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                if (lancamentoRequest.Data == new DateTime())
                    lancamentoRequest.Data = DateTime.Now;

                if (lancamentoRequest.Valor == 0)
                    return BadRequest(new ErroModel("erro_validacao", "Valor não pode ser zerado"));

                var lancamentoEntity = _mapper.Map<Entities.LancamentoEntity>(lancamentoRequest);
                _service.Cadastrar(lancamentoEntity);

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

        [HttpPut("{id_lancamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Alterar(int id_lancamento, LancamentoModelInput lancamentoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                if (lancamentoRequest.Data == new DateTime())
                    lancamentoRequest.Data = DateTime.Now;

                if (lancamentoRequest.Valor == 0)
                    return BadRequest(new ErroModel("erro_validacao", "Valor não pode ser zerado"));

                var lancamentoEntity = _mapper.Map<Entities.LancamentoEntity>(lancamentoRequest);
                lancamentoEntity.Id = id_lancamento;

                _service.Alterar(lancamentoEntity);

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

        [HttpGet("{id_lancamento}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Buscar(int id_lancamento)
        {
            try
            {
                var retorno = _service.Buscar(id_lancamento);

                if (retorno != null)
                    return Ok(_mapper.Map<LancamentoModel>(retorno));
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
        public IActionResult Listar(int id_subcategoria, DateTime data_inicio, DateTime data_fim, string crescente, string decrescente, int? inicio = 1, int? limite = 10)
        {
            try
            {
                ListaPaginada<LancamentoEntity> retorno = _service.Listar(new ParametrosConsultaModel
                {
                    IdSubcategoria = id_subcategoria,
                    DataInicio = data_inicio,
                    DataFim = data_fim,
                    Crescente = crescente,
                    Decrescente = decrescente,
                    Inicio = inicio.GetValueOrDefault(),
                    Limite = limite.GetValueOrDefault(),
                });

                if (retorno != null && retorno.Itens.Count != 0)
                    return Ok(_mapper.Map<LancamentoMetadadoModel>(retorno));
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

        [HttpDelete("{id_lancamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Deletar(int id_lancamento)
        {
            try
            {
                if (!_service.ExisteId(id_lancamento))
                    return new NotFoundObjectResult(new ErroModel("erro_validacao", "A lancamento que deseja excluir não existe na base de dados"));

                var categoriaDeletado = _service.Buscar(id_lancamento);

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