using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Domain.Entidades;
using GestaoClientes.Domain.ObjetosDeValor;

namespace GestaoClientes.Application.Abstracoes;

public interface IRepositorioCliente
{
    Task AdicionarAsync(Cliente cliente, CancellationToken ct);
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExisteCnpjAsync(Cnpj cnpj, CancellationToken ct);
}
