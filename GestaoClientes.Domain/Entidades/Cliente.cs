using GestaoClientes.Domain.ObjetosDeValor;

namespace GestaoClientes.Domain.Entidades;


public class Cliente
{
    public Guid Id { get; private set; }
    public string NomeFantasia { get; private set; } = string.Empty;
    public Cnpj Cnpj { get; private set; }
    public bool Ativo { get; private set; }

    private Cliente() { }

    private Cliente(Guid id, string nomeFantasia, Cnpj cnpj)
    {
        Id = id;
        DefinirNomeFantasia(nomeFantasia);
        Cnpj = cnpj;
        Ativo = true;
    }

    public static Cliente Criar(string nomeFantasia, Cnpj cnpj)
        => new(Guid.NewGuid(), nomeFantasia, cnpj);

    public void DefinirNomeFantasia(string nomeFantasia)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome fantasia é obrigatório.", nameof(nomeFantasia));

        NomeFantasia = nomeFantasia.Trim();
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
}
