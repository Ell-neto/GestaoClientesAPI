using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Domain.Entidades;
using GestaoClientes.Domain.ObjetosDeValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GestaoClientes.Tests.Fakes;

public class RepositorioClienteFake : IRepositorioCliente
{
    private readonly List<Cliente> _clientes = new();

    public Task AdicionarAsync(Cliente cliente, CancellationToken ct)
    {
        _clientes.Add(cliente);
        return Task.CompletedTask;
    }

    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));

    public Task<bool> ExisteCnpjAsync(Cnpj cnpj, CancellationToken ct)
        => Task.FromResult(_clientes.Any(c => c.Cnpj.Valor == cnpj.Valor));

    public Task AtualizarAsync(Cliente cliente, CancellationToken ct)
    {
        var idx = _clientes.FindIndex(c => c.Id == cliente.Id);
        if (idx >= 0) _clientes[idx] = cliente;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Cliente>> ListarAsync(int pagina, int tamanhoPagina, bool? ativo, string? nome, CancellationToken ct)
    {
        if (pagina <= 0) pagina = 1;
        if (tamanhoPagina <= 0) tamanhoPagina = 10;

        IEnumerable<Cliente> consulta = _clientes;

        if (ativo.HasValue)
            consulta = consulta.Where(c => c.Ativo == ativo.Value);

        if (!string.IsNullOrWhiteSpace(nome))
        {
            var filtro = nome.Trim();
            consulta = consulta.Where(c =>
                c.NomeFantasia.Contains(filtro, StringComparison.OrdinalIgnoreCase));
        }

        var resultado = consulta
            .OrderBy(c => c.NomeFantasia)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToList();

        return Task.FromResult((IReadOnlyList<Cliente>)resultado);
    }


}