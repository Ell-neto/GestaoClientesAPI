using System.Text.RegularExpressions;

namespace GestaoClientes.Domain.ObjetosDeValor;

public readonly record struct Cnpj
{
    public string Valor { get; }

    private Cnpj(string valorSomenteDigitos) => Valor = valorSomenteDigitos;

    public static Cnpj Criar(string valor)
    {
        var somenteDigitos = Regex.Replace(valor ?? "", "[^0-9]", "");

        if (somenteDigitos.Length != 14)
            throw new ArgumentException("CNPJ deve conter 14 dígitos.", nameof(valor));

        if (somenteDigitos.Distinct().Count() == 1)
            throw new ArgumentException("CNPJ inválido.", nameof(valor));

        if (!EhValido(somenteDigitos))
            throw new ArgumentException("CNPJ inválido.", nameof(valor));

        return new Cnpj(somenteDigitos);
    }

    public override string ToString() => Valor;

    private static bool EhValido(string cnpj)
    {
        int[] pesos1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] pesos2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        int digito1 = CalcularDigito(cnpj[..12], pesos1);
        int digito2 = CalcularDigito(cnpj[..12] + digito1, pesos2);

        return cnpj.EndsWith($"{digito1}{digito2}");
    }

    private static int CalcularDigito(string baseNumerica, int[] pesos)
    {
        int soma = 0;
        for (int i = 0; i < pesos.Length; i++)
            soma += (baseNumerica[i] - '0') * pesos[i];

        int resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
