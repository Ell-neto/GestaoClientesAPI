using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Domain.Entidades;
using GestaoClientes.Domain.ObjetosDeValor;

namespace GestaoClientes.Infrastructure.Persistencia;

public class RepositorioClienteEmMemoria : IRepositorioCliente
{
    private static readonly ConcurrentDictionary<Guid, Cliente> _porId = new();
    private static readonly ConcurrentDictionary<string, Guid> _porCnpj = new();

    public Task AdicionarAsync(Cliente cliente, CancellationToken ct)
    {
        if (!_porCnpj.TryAdd(cliente.Cnpj.Valor, cliente.Id))
            throw new InvalidOperationException("Já existe um cliente cadastrado com este CNPJ.");

        if (!_porId.TryAdd(cliente.Id, cliente))
        {
            _porCnpj.TryRemove(cliente.Cnpj.Valor, out _);
            throw new InvalidOperationException("Falha ao salvar cliente em memória.");
        }

        return Task.CompletedTask;
    }

    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        _porId.TryGetValue(id, out var cliente);
        return Task.FromResult(cliente);
    }

    public Task<bool> ExisteCnpjAsync(Cnpj cnpj, CancellationToken ct)
        => Task.FromResult(_porCnpj.ContainsKey(cnpj.Valor));
}
