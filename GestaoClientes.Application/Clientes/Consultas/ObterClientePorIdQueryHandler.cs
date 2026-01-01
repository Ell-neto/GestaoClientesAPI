using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Application.Clientes.DTOs;

namespace GestaoClientes.Application.Clientes.Consultas;

public class ObterClientePorIdQueryHandler
{
    private readonly IRepositorioCliente _repositorio;

    public ObterClientePorIdQueryHandler(IRepositorioCliente repositorio)
        => _repositorio = repositorio;

    public async Task<ClienteDto?> ExecutarAsync(ObterClientePorIdQuery query, CancellationToken ct)
    {
        var cliente = await _repositorio.ObterPorIdAsync(query.Id, ct);
        if (cliente is null) return null;

        return new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj.Valor, cliente.Ativo);
    }
}
