using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Tests.Fakes;

namespace GestaoClientes.Tests;

public class CriarClienteCommandHandlerTests
{
    [Fact]
    public async Task Deve_criar_cliente_com_sucesso_quando_dados_sao_validos()
    {
        var repo = new RepositorioClienteFake();
        var handler = new CriarClienteCommandHandler(repo);

        var cmd = new CriarClienteCommand("ManoelTeste", "11.222.333/0001-81");
        var resultado = await handler.ExecutarAsync(cmd, CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.NotEqual(Guid.Empty, resultado.Valor);
    }

    [Fact]
    public async Task Deve_retornar_erro_se_cnpj_ja_existir()
    {
        var repo = new RepositorioClienteFake();
        var handler = new CriarClienteCommandHandler(repo);

        var cmd1 = new CriarClienteCommand("Cliente Novo", "11.222.333/0001-81");
        var cmd2 = new CriarClienteCommand("Cliente Segundo Novo", "11.222.333/0001-81");

        var r1 = await handler.ExecutarAsync(cmd1, CancellationToken.None);
        var r2 = await handler.ExecutarAsync(cmd2, CancellationToken.None);

        Assert.True(r1.Sucesso);
        Assert.False(r2.Sucesso);
        Assert.Contains("Já existe um cliente cadastrado com este CNPJ.", r2.Erros);
    }

    [Fact]
    public async Task Deve_retornar_erro_se_nome_for_invalido()
    {
        var repo = new RepositorioClienteFake();
        var handler = new CriarClienteCommandHandler(repo);

        var cmd = new CriarClienteCommand("   ", "11.222.333/0001-81");
        var resultado = await handler.ExecutarAsync(cmd, CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Nome fantasia é obrigatório.", resultado.Erros);
    }
}
