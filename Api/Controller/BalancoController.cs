using System;
using System.Linq;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controller
{
    [ApiController]
    [Route("/v1/balanco")]
    [EnableCors("AllowOrigin")]
    public class BalancoController : ControllerBase
    {
        private const string MSG_ERRO_500 = "Ocorreu um erro inesperado";
        private readonly ILogger<BalancoController> _logger;
        private readonly IBalancoService _service;

        public BalancoController(ILogger<BalancoController> logger
                                     , IBalancoService service)
        {
            _logger = logger;

            _service = service ??
                throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BalancoModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [Produces("application/json")]
        public IActionResult Balanco(int id_categoria, DateTime data_inicio, DateTime data_fim)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroModel("", string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                if (data_inicio == new DateTime() || data_fim == new DateTime())
                    throw new ArgumentNullException("Datas são obrigatórias");

                var retorno = _service.Calcular(new ParametrosConsultaModel
                {
                    IdCategoria = id_categoria,
                    DataInicio = data_inicio,
                    DataFim = data_fim
                });

                if (retorno != null)
                    return Ok(retorno);
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
    }
}