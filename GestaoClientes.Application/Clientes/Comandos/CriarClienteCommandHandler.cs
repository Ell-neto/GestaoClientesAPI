using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Domain.Entidades;
using GestaoClientes.Domain.ObjetosDeValor;

namespace GestaoClientes.Application.Clientes.Comandos;

public class CriarClienteCommandHandler
{
    private readonly IRepositorioCliente _repositorio;

    public CriarClienteCommandHandler(IRepositorioCliente repositorio)
        => _repositorio = repositorio;

    public async Task<Resultado<Guid>> ExecutarAsync(CriarClienteCommand comando, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(comando.NomeFantasia))
            return Resultado<Guid>.Falha("Nome fantasia é obrigatório.");

        Cnpj cnpj;
        try
        {
            cnpj = Cnpj.Criar(comando.Cnpj);
        }
        catch (ArgumentException ex)
        {
            return Resultado<Guid>.Falha(ex.Message);
        }

        if (await _repositorio.ExisteCnpjAsync(cnpj, ct))
            return Resultado<Guid>.Falha("Já existe um cliente cadastrado com este CNPJ.");

        Cliente cliente;
        try
        {
            cliente = Cliente.Criar(comando.NomeFantasia, cnpj);
        }
        catch (ArgumentException ex)
        {
            return Resultado<Guid>.Falha(ex.Message);
        }

        await _repositorio.AdicionarAsync(cliente, ct);
        return Resultado<Guid>.Ok(cliente.Id);
    }
}
