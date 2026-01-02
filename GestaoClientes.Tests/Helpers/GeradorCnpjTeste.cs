using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Tests.Helpers
{
    public static class GeradorCnpjTeste
    {
        public static string GerarCnpjValido(string base12)
        {
            if (string.IsNullOrWhiteSpace(base12))
                throw new ArgumentException("Base deve ser informada.");

            var somenteDigitos = new string(base12.Where(char.IsDigit).ToArray());
            if (somenteDigitos.Length != 12)
                throw new ArgumentException("A base deve ter 12 dígitos.");

            int[] pesos1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] pesos2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            int dig1 = CalcularDigito(somenteDigitos, pesos1);
            int dig2 = CalcularDigito(somenteDigitos + dig1, pesos2);

            return somenteDigitos + dig1 + dig2;
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
}
