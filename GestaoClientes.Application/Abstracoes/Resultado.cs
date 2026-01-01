namespace GestaoClientes.Application.Abstracoes;

public class Resultado<T>
{
    public bool Sucesso { get; }
    public T? Valor { get; }
    public IReadOnlyList<string> Erros { get; }

    private Resultado(bool sucesso, T? valor, IReadOnlyList<string> erros)
        => (Sucesso, Valor, Erros) = (sucesso, valor, erros);

    public static Resultado<T> Ok(T valor) => new(true, valor, Array.Empty<string>());
    public static Resultado<T> Falha(params string[] erros) => new(false, default, erros);
}
