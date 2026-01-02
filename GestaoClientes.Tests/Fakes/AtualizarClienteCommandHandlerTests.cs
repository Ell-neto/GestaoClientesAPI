using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Tests.Fakes;

namespace GestaoClientes.Tests;

public class AtualizarClienteCommandHandlerTests
{
    [Fact]
    public async Task Deve_atualizar_cliente_quando_dados_sao_validos()
    {
        var repo = new RepositorioClienteFake();
        var criar = new CriarClienteCommandHandler(repo);

        var criado = await criar.ExecutarAsync(
            new CriarClienteCommand("ManoelTeste", "11.222.333/0001-81"),
            CancellationToken.None);

        var id = criado.Valor!;

        var handler = new AtualizarClienteCommandHandler(repo);

        var resultado = await handler.ExecutarAsync(
            new AtualizarClienteCommand(id, "Cliente Atualizado", false),
            CancellationToken.None);

        Assert.True(resultado.Sucesso);

        var obtido = await repo.ObterPorIdAsync(id, CancellationToken.None);
        Assert.NotNull(obtido);
        Assert.Equal("Cliente Atualizado", obtido!.NomeFantasia);
        Assert.False(obtido.Ativo);
    }

    [Fact]
    public async Task Deve_retornar_erro_quando_cliente_nao_existe()
    {
        var repo = new RepositorioClienteFake();
        var handler = new AtualizarClienteCommandHandler(repo);

        var resultado = await handler.ExecutarAsync(
            new AtualizarClienteCommand(Guid.NewGuid(), "X", true),
            CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Cliente não encontrado.", resultado.Erros);
    }
}
