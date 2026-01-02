using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;

namespace GestaoClientes.Application.Clientes.Comandos;

public class AtualizarClienteCommandHandler
{
    private readonly IRepositorioCliente _repositorio;

    public AtualizarClienteCommandHandler(IRepositorioCliente repositorio)
        => _repositorio = repositorio;

    public async Task<Resultado<bool>> ExecutarAsync(AtualizarClienteCommand comando, CancellationToken ct)
    {
        var cliente = await _repositorio.ObterPorIdAsync(comando.Id, ct);
        if (cliente is null)
            return Resultado<bool>.Falha("Cliente não encontrado.");

        try
        {
            cliente.DefinirNomeFantasia(comando.NomeFantasia);
        }
        catch (ArgumentException ex)
        {
            return Resultado<bool>.Falha(ex.Message);
        }

        if (comando.Ativo) cliente.Ativar();
        else cliente.Desativar();

        await _repositorio.AtualizarAsync(cliente, ct);
        return Resultado<bool>.Ok(true);
    }
}
