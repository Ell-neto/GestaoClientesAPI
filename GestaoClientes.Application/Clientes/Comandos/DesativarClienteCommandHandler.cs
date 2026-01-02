using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;

namespace GestaoClientes.Application.Clientes.Comandos;

public class DesativarClienteCommandHandler
{
    private readonly IRepositorioCliente _repositorio;

    public DesativarClienteCommandHandler(IRepositorioCliente repositorio)
        => _repositorio = repositorio;

    public async Task<Resultado<bool>> ExecutarAsync(DesativarClienteCommand comando, CancellationToken ct)
    {
        var cliente = await _repositorio.ObterPorIdAsync(comando.Id, ct);
        if (cliente is null)
            return Resultado<bool>.Falha("Cliente não encontrado.");

        cliente.Desativar();

        await _repositorio.AtualizarAsync(cliente, ct);

        return Resultado<bool>.Ok(true);
    }
}
