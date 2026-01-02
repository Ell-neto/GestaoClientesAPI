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
}