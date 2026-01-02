using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Application.Clientes.Consultas;
using GestaoClientes.Tests.Fakes;

namespace GestaoClientes.Tests;

public class ObterClientePorIdQueryHandlerTests
{
    [Fact]
    public async Task Deve_retornar_o_cliente_correto_quando_id_existe()
    {
        var repo = new RepositorioClienteFake();

        var criar = new CriarClienteCommandHandler(repo);
        var criarResultado = await criar.ExecutarAsync(
            new CriarClienteCommand("Cliente Teste", "11.222.333/0001-81"),
            CancellationToken.None);

        var id = criarResultado.Valor!;

        var obter = new ObterClientePorIdQueryHandler(repo);
        var dto = await obter.ExecutarAsync(new ObterClientePorIdQuery(id), CancellationToken.None);

        Assert.NotNull(dto);
        Assert.Equal(id, dto!.Id);
        Assert.Equal("Cliente Teste", dto.NomeFantasia);
    }

    [Fact]
    public async Task Deve_retornar_nulo_quando_id_nao_existe()
    {
        var repo = new RepositorioClienteFake();
        var handler = new ObterClientePorIdQueryHandler(repo);

        var dto = await handler.ExecutarAsync(new ObterClientePorIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.Null(dto);
    }
}
