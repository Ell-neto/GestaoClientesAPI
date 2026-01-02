using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Application.Clientes.DTOs;

namespace GestaoClientes.Application.Clientes.Consultas;

public class ListarClientesQueryHandler
{
    private readonly IRepositorioCliente _repositorio;

    public ListarClientesQueryHandler(IRepositorioCliente repositorio)
        => _repositorio = repositorio;

    public async Task<IReadOnlyList<ClienteDto>> ExecutarAsync(ListarClientesQuery query, CancellationToken ct)
    {
        var clientes = await _repositorio.ListarAsync(query.Pagina, query.TamanhoPagina, query.Ativo, query.Nome, ct);

        return clientes
            .Select(c => new ClienteDto(c.Id, c.NomeFantasia, c.Cnpj.Valor, c.Ativo))
            .ToList();
    }
}
