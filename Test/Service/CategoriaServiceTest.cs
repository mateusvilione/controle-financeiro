using System.Security.Cryptography.X509Certificates;
using Api.Services;
using Api.Repository;
using Moq;
using Api.Entities;
using Xunit;
using System;

namespace Test.Services
{
    public class CategoriaServiceTest
    {
        private readonly CategoriaService _service;
        private readonly Mock<ICategoriaRepository> _repository;
        private readonly Mock<IPaginarService<CategoriaEntity>> _paginarService;

        public CategoriaServiceTest()
        {
            _repository = new Mock<ICategoriaRepository>();
            _paginarService = new Mock<IPaginarService<CategoriaEntity>>();
            _service = new CategoriaService(_paginarService.Object, _repository.Object);
        }

        #region Teste de instancia da classe
        [Fact]
        public void InstanciaErroPaginarService()
        {
            try
            {
                var instancia = new CategoriaService(null, _repository.Object);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'paginarService')", e.Message);
            }
        }

        [Fact]
        public void InstanciaErroRepository()
        {
            try
            {
                var instancia = new CategoriaService(_paginarService.Object, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'repository')", e.Message);
            }
        }
        #endregion

        #region Cadastro
        [Fact]
        public void CadastrarErro()
        {
            _repository.Setup(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>())).Returns(true);

            try
            {
                _service.Cadastrar(new CategoriaEntity()
                {
                    Id = 0,
                    Nome = "Comida"
                });
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentException>(e);
                Assert.Equal("Categoria já cadastrada", e.Message);
            }
            _repository.Verify(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Once);

            _repository.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void CadastrarErroNull()
        {
            try
            {
                _service.Cadastrar(null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
                Assert.Equal("Value cannot be null. (Parameter 'categoriaEntity')", e.Message);
            }
            _repository.Verify(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Never);

            _repository.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Never);
        }

        [Fact]
        public void CadastrarSucesso()
        {
            _repository.Setup(x =>
                x.Salvar()).Returns(true);

            _service.Cadastrar(new CategoriaEntity()
            {
                Id = 0,
                Nome = "Comida"
            });

            _repository.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Once);

            _repository.Verify(x =>
                x.Salvar(), Times.Once);
        }

        [Fact]
        public void CadastrarErroSalvar()
        {
            try
            {
                _service.Cadastrar(new CategoriaEntity()
                {
                    Id = 0,
                    Nome = "Comida"
                });
            }
            catch (Exception e)
            {
                Assert.IsType<Exception>(e);
                Assert.Equal("Não foi possível salvar", e.Message);
            }

            _repository.Verify(x =>
                x.Cadastrar(It.IsAny<CategoriaEntity>()), Times.Once);

            _repository.Verify(x =>
                x.Salvar(), Times.Once);
        }
        #endregion

        [Fact]
        public void ExisteNome()
        {
            _repository.Setup(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>())).Returns(true);

            var resultado = _service.ExisteNome(new CategoriaEntity
            {
                Id = 1,
                Nome = "Comida"
            });

            Assert.Equal(true, resultado);

            _repository.Verify(x =>
                x.ExisteNome(It.IsAny<CategoriaEntity>()), Times.Once);
        }

        [Fact]
        public void ExisteId()
        {
            _repository.Setup(x =>
                x.ExisteId(It.IsAny<int>())).Returns(true);

            var resultado = _service.ExisteId(1);

            Assert.Equal(true, resultado);

            _repository.Verify(x =>
                x.ExisteId(It.IsAny<int>()), Times.Once);
        }
    }
}