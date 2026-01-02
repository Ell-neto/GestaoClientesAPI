using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Application.Clientes.Consultas;
using GestaoClientes.Tests.Fakes;
using GestaoClientes.Tests.Helpers;

namespace GestaoClientes.Tests;

public class ListarClientesQueryHandlerTests
{
    [Fact]
    public async Task Deve_listar_clientes_com_paginacao()
    {
        var repo = new RepositorioClienteFake();
        var criar = new CriarClienteCommandHandler(repo);

        // Criando para meu teste os 3 clientes (todos ativos por padrão)
        await criar.ExecutarAsync(new CriarClienteCommand("ManoelTeste", GeradorCnpjTeste.GerarCnpjValido("123456780001")), CancellationToken.None);
        await criar.ExecutarAsync(new CriarClienteCommand("Cliente Boa sorte", GeradorCnpjTeste.GerarCnpjValido("234567890001")), CancellationToken.None);
        await criar.ExecutarAsync(new CriarClienteCommand("Cliente Mano", GeradorCnpjTeste.GerarCnpjValido("345678900001")), CancellationToken.None);

        var handler = new ListarClientesQueryHandler(repo);

        var pagina1 = await handler.ExecutarAsync(new ListarClientesQuery(Pagina: 1, TamanhoPagina: 2), CancellationToken.None);
        var pagina2 = await handler.ExecutarAsync(new ListarClientesQuery(Pagina: 2, TamanhoPagina: 2), CancellationToken.None);

        Assert.Equal(2, pagina1.Count);
        Assert.Single(pagina2);
    }

    [Fact]
    public async Task Deve_filtrar_por_ativo()
    {
        var repo = new RepositorioClienteFake();
        var criar = new CriarClienteCommandHandler(repo);

        var r1 = await criar.ExecutarAsync(new CriarClienteCommand("Ativo 1", GeradorCnpjTeste.GerarCnpjValido("123456780001")), CancellationToken.None);
        var r2 = await criar.ExecutarAsync(new CriarClienteCommand("Ativo 2", GeradorCnpjTeste.GerarCnpjValido("234567890001")), CancellationToken.None);

        var desativar = new DesativarClienteCommandHandler(repo);
        await desativar.ExecutarAsync(new DesativarClienteCommand(r2.Valor!), CancellationToken.None);

        var handler = new ListarClientesQueryHandler(repo);

        var apenasAtivos = await handler.ExecutarAsync(new ListarClientesQuery(Pagina: 1,TamanhoPagina: 10, Ativo: true), CancellationToken.None);
        var apenasInativos = await handler.ExecutarAsync(new ListarClientesQuery(Pagina: 1, TamanhoPagina: 10, Ativo: false), CancellationToken.None);

        Assert.Single(apenasAtivos);
        Assert.Single(apenasInativos);

        Assert.True(apenasAtivos[0].Ativo);
        Assert.False(apenasInativos[0].Ativo);
    }

    [Fact]
    public async Task Deve_filtrar_por_nome_contendo()
    {
        var repo = new RepositorioClienteFake();
        var criar = new CriarClienteCommandHandler(repo);

        await criar.ExecutarAsync(new CriarClienteCommand("Decora Lojinha", GeradorCnpjTeste.GerarCnpjValido("123456780001")), CancellationToken.None);
        //await criar.ExecutarAsync(new CriarClienteCommand("Mercado Decora", GeradorCnpjTeste.GerarCnpjValido("234567890001")), CancellationToken.None);
        await criar.ExecutarAsync(new CriarClienteCommand("Mercado Central", GeradorCnpjTeste.GerarCnpjValido("234567890001")), CancellationToken.None);
        await criar.ExecutarAsync(new CriarClienteCommand("Manel Tec", GeradorCnpjTeste.GerarCnpjValido("345678900001")), CancellationToken.None);

        var handler = new ListarClientesQueryHandler(repo);

        var lista = await handler.ExecutarAsync(
            new ListarClientesQuery(Pagina: 1, TamanhoPagina: 10, Nome: "decora"),
            CancellationToken.None);

        Assert.Single(lista);
        //Assert.Equal(2, lista.Count);  //Para meu teste com 2 nomes com "decora"
        Assert.All(lista, c => Assert.Contains("decora", c.NomeFantasia, StringComparison.OrdinalIgnoreCase));
    }
}
