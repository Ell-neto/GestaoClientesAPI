using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Tests.Fakes;

namespace GestaoClientes.Tests;

public class DesativarClienteCommandHandlerTests
{
    [Fact]
    public async Task Deve_desativar_cliente_quando_id_existe()
    {
        var repo = new RepositorioClienteFake();

        var criar = new CriarClienteCommandHandler(repo);
        var criado = await criar.ExecutarAsync(
            new CriarClienteCommand("ManoelTeste", "11.222.333/0001-81"),
            CancellationToken.None);

        var id = criado.Valor!;

        var handler = new DesativarClienteCommandHandler(repo);
        var resultado = await handler.ExecutarAsync(new DesativarClienteCommand(id), CancellationToken.None);

        Assert.True(resultado.Sucesso);

        var cliente = await repo.ObterPorIdAsync(id, CancellationToken.None);
        Assert.NotNull(cliente);
        Assert.False(cliente!.Ativo);
    }

    [Fact]
    public async Task Deve_retornar_erro_quando_id_nao_existe()
    {
        var repo = new RepositorioClienteFake();
        var handler = new DesativarClienteCommandHandler(repo);

        var resultado = await handler.ExecutarAsync(new DesativarClienteCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Cliente não encontrado.", resultado.Erros);
    }
}
