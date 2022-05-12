using System;
using System.Collections.Generic;
using Api.Controller;
using Api.Entities;
using Api.Models;
using Api.Profiles;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Controller
{
    public class CategoriaControllerTest
    {
        private const int _statusCodeExcecao = 500;
        private const string _mensagemErroExcecao = "Ocorreu um erro inesperado";
        private const string MSG_ERRO_500 = "Ocorreu um erro inesperado";
        private readonly CategoriaController _controller;
        private readonly Mock<ILogger<CategoriaController>> _logger;
        private readonly Mock<ICategoriaService> _service;
        private readonly IMapper _mapper;
        private readonly Exception _exception;
        private readonly ArgumentException _argumentException;

        public CategoriaControllerTest()
        {
            var mapperProfiles = new MapperProfiles();

            var configuration = new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(mapperProfiles);
                });

            var _mapper = new Mapper(configuration);

            _logger = new Mock<ILogger<CategoriaController>>();
            _service = new Mock<ICategoriaService>();
            _exception = new Exception(MSG_ERRO_500);
            _argumentException = new ArgumentException(MSG_ERRO_500);
            _controller = new CategoriaController(_logger.Object, _service.Object, _mapper);
        }

        #region Teste de instancia da classe
        [Fact]
        public void InstanciaErrologger()
        {
            try
            {
                var instancia = new CategoriaController(null, _service.Object, _mapper);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'logger')", e.Message);
            }
        }

        [Fact]
        public void InstanciaErroService()
        {
            try
            {
                var instancia = new CategoriaController(_logger.Object, null, _mapper);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'service')", e.Message);
            }
        }

        [Fact]
        public void InstanciaErroMapper()
        {
            try
            {
                var instancia = new CategoriaController(_logger.Object, _service.Object, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'mapper')", e.Message);
            }
        }
        #endregion

        #region rota Cadastro
        [Fact]
        public void InclusaoErro400PorModelState()
        {
            string mensagemErro = "Test Error";

            _controller.ModelState.AddModelError("x", mensagemErro);

            IActionResult resultado = _controller.Cadastrar(new CategoriaModelInput() { });

            var resposta = Assert.IsType<BadRequestObjectResult>(resultado);
            var conteudo = Assert.IsType<ErroModel>(resposta.Value);

            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(mensagemErro, conteudo.Mensagem);

            _service.Verify(
                x => x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void InclusaoErro400()
        {
            _service.Setup(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>())).Throws(new ArgumentException("Categoria já cadastrada"));

            IActionResult resultado = _controller.Cadastrar(new CategoriaModelInput()
            {
                Nome = "Comida"
            });

            var resposta = Assert.IsType<BadRequestObjectResult>(resultado);
            var conteudo = Assert.IsType<ErroModel>(resposta.Value);

            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal("Categoria já cadastrada", conteudo.Mensagem);

            _service.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Once);
        }

        [Fact]
        public void Inclusao201()
        {
            _service.Setup(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()));

            IActionResult resultado = _controller.Cadastrar(new CategoriaModelInput()
            {
                Nome = "comida"
            });

            var resposta = Assert.IsType<CreatedAtActionResult>(resultado);

            _service.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Once);
        }

        [Fact]
        public void InclusaoErro500()
        {
            _service.Setup(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>())).Throws(_exception);

            IActionResult resultado = _controller.Cadastrar(new CategoriaModelInput()
            {
                Nome = "comida"
            });

            var resposta = Assert.IsType<JsonResult>(resultado);

            Assert.NotNull(resposta.Value);
            Assert.NotNull(resposta.SerializerSettings);

            var status = Assert.IsType<int>(resposta.Value);
            var conteudo = Assert.IsType<ErroModel>(resposta.SerializerSettings);

            Assert.Equal(_statusCodeExcecao, status);
            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(_mensagemErroExcecao, conteudo.Mensagem);

            _service.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Once);
        }
        #endregion

        #region rota Alteracao
        [Fact]
        public void AlteracaoErro400PorModelState()
        {
            string mensagemErro = "Test Error";

            _controller.ModelState.AddModelError("x", mensagemErro);

            IActionResult resultado = _controller.Alterar(1, new CategoriaModelInput() { });

            var resposta = Assert.IsType<BadRequestObjectResult>(resultado);
            var conteudo = Assert.IsType<ErroModel>(resposta.Value);

            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(mensagemErro, conteudo.Mensagem);

            _service.Verify(
                x => x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Never);

            _service.Verify(
                x => x.Alterar(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void AlteracaoErro409()
        {
            _service.Setup(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>())).Returns(true);

            IActionResult resultado = _controller.Alterar(1, new CategoriaModelInput()
            {
                Nome = "Comida"
            });

            var resposta = Assert.IsType<ConflictObjectResult>(resultado);
            var conteudo = Assert.IsType<ErroModel>(resposta.Value);

            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal("O nome já está sendo utilizado", conteudo.Mensagem);

            _service.Verify(
                 x => x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Once);

            _service.Verify(
                x => x.Alterar(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void Alteracao200()
        {
            _service.Setup(x =>
                 x.ExisteNome(It.IsAny<CategoriaEntity>())).Returns(false);

            _service.Setup(x =>
                x.Alterar(It.IsAny<CategoriaEntity>()));

            IActionResult resultado = _controller.Alterar(1, new CategoriaModelInput()
            {
                Nome = "Bebida"
            });

            var resposta = Assert.IsType<OkResult>(resultado);

            _service.Verify(
                  x => x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Once);

            _service.Verify(
                x => x.Alterar(It.IsAny<CategoriaEntity>()), Times.Once);
        }

        [Fact]
        public void AlteracaoErro500()
        {
            _service.Setup(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>())).Returns(false);

            _service.Setup(x =>
                x.Alterar(It.IsAny<CategoriaEntity>())).Throws(_exception);

            IActionResult resultado = _controller.Alterar(1, new CategoriaModelInput()
            {
                Nome = "Bebida"
            });

            var resposta = Assert.IsType<JsonResult>(resultado);

            Assert.NotNull(resposta.Value);
            Assert.NotNull(resposta.SerializerSettings);

            var status = Assert.IsType<int>(resposta.Value);
            var conteudo = Assert.IsType<ErroModel>(resposta.SerializerSettings);

            Assert.Equal(_statusCodeExcecao, status);
            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(_mensagemErroExcecao, conteudo.Mensagem);

            _service.Verify(x =>
                x.Alterar(It.IsAny<CategoriaEntity>()), Times.Once);
        }
        #endregion

        #region rota Buscar
        [Fact]
        public void Buscar200()
        {
            _service.Setup(x =>
                x.Buscar(It.IsAny<int>())).Returns(new CategoriaEntity
                {
                    Id = 1,
                    Nome = "Comida"
                });

            IActionResult resultado = _controller.Buscar(1);

            var resposta = Assert.IsType<OkObjectResult>(resultado);

            _service.Verify(
                x => x.Buscar(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Buscar204()
        {
            _service.Setup(x =>
                x.Buscar(It.IsAny<int>())).Returns(() => null);

            IActionResult resultado = _controller.Buscar(1);

            var resposta = Assert.IsType<NoContentResult>(resultado);

            _service.Verify(
                x => x.Buscar(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void BuscarErro500()
        {
            _service.Setup(x =>
                x.Buscar(It.IsAny<int>())).Throws(_exception);

            IActionResult resultado = _controller.Buscar(1);

            var resposta = Assert.IsType<JsonResult>(resultado);

            Assert.NotNull(resposta.Value);
            Assert.NotNull(resposta.SerializerSettings);

            var status = Assert.IsType<int>(resposta.Value);
            var conteudo = Assert.IsType<ErroModel>(resposta.SerializerSettings);

            Assert.Equal(_statusCodeExcecao, status);
            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(_mensagemErroExcecao, conteudo.Mensagem);

            _service.Verify(x =>
                x.Buscar(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region rota Listar
        [Fact]
        public void Listar200()
        {
            _service.Setup(x =>
                x.Listar(It.IsAny<ParametrosConsultaModel>())).Returns(() => new ListaPaginada<CategoriaEntity>(new List<CategoriaEntity> {
                    new CategoriaEntity{
                        Id = 1,
                        Nome = "Comida"
                    },
                    new CategoriaEntity{
                        Id = 2,
                        Nome = "Bebida"
                    }
                }, 2, 1, 10));

            IActionResult resultado = _controller.Listar("", "", "", 1, 10);

            var resposta = Assert.IsType<OkObjectResult>(resultado);

            _service.Verify(
                x => x.Listar(It.IsAny<ParametrosConsultaModel>()), Times.Once);
        }

        [Fact]
        public void Listar204()
        {
            _service.Setup(x =>
                x.Listar(It.IsAny<ParametrosConsultaModel>())).Returns(() => null);

            IActionResult resultado = _controller.Listar("", "", "", 1, 10);

            var resposta = Assert.IsType<NoContentResult>(resultado);

            _service.Verify(
                x => x.Listar(It.IsAny<ParametrosConsultaModel>()), Times.Once);
        }

        [Fact]
        public void ListarErro500()
        {
            _service.Setup(x =>
                x.Listar(It.IsAny<ParametrosConsultaModel>())).Throws(_exception);

            IActionResult resultado = _controller.Listar("", "", "", 1, 10);

            var resposta = Assert.IsType<JsonResult>(resultado);

            Assert.NotNull(resposta.Value);
            Assert.NotNull(resposta.SerializerSettings);

            var status = Assert.IsType<int>(resposta.Value);
            var conteudo = Assert.IsType<ErroModel>(resposta.SerializerSettings);

            Assert.Equal(_statusCodeExcecao, status);
            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(_mensagemErroExcecao, conteudo.Mensagem);

            _service.Verify(x =>
                x.Listar(It.IsAny<ParametrosConsultaModel>()), Times.Once);
        }
        #endregion

        #region rota cadastro
        [Fact]
        public void Inclusao404()
        {
            _service.Setup(x =>
                x.ExisteId(It.IsAny<int>())).Returns(false);

            IActionResult resultado = _controller.Deletar(1);

            var resposta = Assert.IsType<NotFoundObjectResult>(resultado);

            _service.Verify(x =>
                 x.ExisteId(It.IsAny<int>()), Times.Once);

            _service.Verify(x =>
                 x.Buscar(It.IsAny<int>()), Times.Never);

            _service.Verify(x =>
                x.Excluir(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void Inclusao200()
        {
            _service.Setup(x =>
                x.ExisteId(It.IsAny<int>())).Returns(true);

            _service.Setup(x =>
                x.Buscar(It.IsAny<int>())).Returns(new CategoriaEntity
                {
                    Id = 1,
                    Nome = "Comida"
                });

            IActionResult resultado = _controller.Deletar(1);

            var resposta = Assert.IsType<OkResult>(resultado);

            _service.Verify(x =>
                 x.ExisteId(It.IsAny<int>()), Times.Once);

            _service.Verify(x =>
                 x.Buscar(It.IsAny<int>()), Times.Once);

            _service.Verify(x =>
                x.Excluir(It.IsAny<CategoriaEntity>()), Times.Once);
        }

        [Fact]
        public void DeletarErro500()
        {
            _service.Setup(x =>
                  x.ExisteId(It.IsAny<int>())).Throws(_exception);

            IActionResult resultado = _controller.Deletar(1);

            var resposta = Assert.IsType<JsonResult>(resultado);

            Assert.NotNull(resposta.Value);
            Assert.NotNull(resposta.SerializerSettings);

            var status = Assert.IsType<int>(resposta.Value);
            var conteudo = Assert.IsType<ErroModel>(resposta.SerializerSettings);

            Assert.Equal(_statusCodeExcecao, status);
            Assert.NotNull(conteudo);
            Assert.NotNull(conteudo.Mensagem);
            Assert.Equal(_mensagemErroExcecao, conteudo.Mensagem);

            _service.Verify(x =>
                 x.ExisteId(It.IsAny<int>()), Times.Once);

            _service.Verify(x =>
                 x.Buscar(It.IsAny<int>()), Times.Never);

            _service.Verify(x =>
                x.Excluir(It.IsAny<CategoriaEntity>()), Times.Never);
        }
        #endregion
    }
}