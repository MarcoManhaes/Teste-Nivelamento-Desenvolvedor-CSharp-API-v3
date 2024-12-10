using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Sqlite;
using FluentAssertions;
using Xunit;
using Dapper;
using Questao5.Domain.Enumerators.Questao5.Domain.Enumerators;
using System.Data;

namespace Questao5.Tests.Handlers
{
    public class MovimentacaoHandlerTests
    {
        private readonly IContaCorrenteRepository _repositoryMock;
        private readonly MovimentacaoHandler _handler;

        public MovimentacaoHandlerTests()
        {
            _repositoryMock = Substitute.For<IContaCorrenteRepository>();
            _handler = new MovimentacaoHandler(_repositoryMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenTipoMovimentoIsInvalid()
        {
            // Arrange
            var request = new MovimentacaoRequest
            {
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 310,
                TipoMovimento = (TipoMovimento)999, // Tipo inválido
                Idempotencia = "chave_teste"
            };

            // Mock de conta válida
            _repositoryMock.GetContaCorrente(Arg.Any<string>()).Returns(new { Ativo = 1 }); // Conta válida

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            result.Error.Should().Be("Tipo de movimento inválido");
            result.ErrorType.Should().Be("INVALID_TYPE");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenContaCorrenteIsInvalid()
        {
            // Arrange
            var request = new MovimentacaoRequest
            {
                IdContaCorrente = "INVALID_GUID",
                Valor = 310,
                TipoMovimento = TipoMovimento.C,
                Idempotencia = "chave_teste"
            };

            _repositoryMock.GetContaCorrente(Arg.Any<string>()).Returns(null); // Conta inexistente

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            result.Error.Should().Be("Conta corrente não cadastrada ou inativa");
            result.ErrorType.Should().Be("INVALID_ACCOUNT");
        }

        [Fact]
        public async Task Handle_ShouldProcessSuccessfully_WhenRequestIsValid()
        {
            // Arrange
            var request = new MovimentacaoRequest
            {
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Valor = 310,
                TipoMovimento = TipoMovimento.C,
                Idempotencia = "nova_chave"
            };

            _repositoryMock.GetContaCorrente(Arg.Any<string>()).Returns(new { Ativo = 1 }); // Conta válida

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            result.IdMovimento.Should().NotBeNullOrEmpty();
            result.Error.Should().BeNull();
        }
    }
}