using System;
using System.Text.RegularExpressions;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public static class ValidationHelper
    {
        private static readonly Regex RegexEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@(([A-Za-z\d][A-Za-z\d-]{0,61}[A-Za-z\d]\.)+[A-Za-z]{2,6}|\[\d{1,3}(\.\d{1,3}){3}\])$", RegexOptions.Compiled);

        private static readonly Regex RegexPlaca = new Regex(@"^[A-Za-z\d]{7,8}$", RegexOptions.Compiled);

        private static readonly Regex RegexPlacaMercosul = new Regex(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", RegexOptions.Compiled);

        private static readonly Regex RegexIMEI = new Regex(@"^[A-Za-z0-9]{15,18}$", RegexOptions.Compiled);

        public static bool ValidarCpf(string cpf)
        {
            // recupera os digitos do cpf separadamente
            var digitos = ObterDigitos(cpf);

            // todos os números são iguais?
            if (digitos == null || digitos.Length != 11 || ValidarNumerosIguais(digitos))
            {
                // CPF inválido
                return false;
            }

            var dv1 = CalcularDigitoVerificadorMod11(digitos, new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            // verifica o DV1
            if (digitos[9] != dv1)
            {
                return false;
            }

            var dv2 = CalcularDigitoVerificadorMod11(digitos, new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            // Verifica o DV
            if (digitos[10] != dv2)
            {
                return false;
            }

            // CPF Válido!
            return true;
        }

        public static bool ValidarCnpj(string cnpj)
        {
            // recupera os digitos do CNPJ separadamente
            var digitos = ObterDigitos(cnpj);

            // todos os números são iguais?
            if (digitos == null || digitos.Length != 14 || ValidarNumerosIguais(digitos))
            {
                // CNPJ inválido
                return false;
            }

            var dv1 = CalcularDigitoVerificadorMod11(digitos, new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            // verifica o DV1
            if (digitos[12] != dv1)
            {
                return false;
            }

            var dv2 = CalcularDigitoVerificadorMod11(digitos, new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            // Verifica o DV
            if (digitos[13] != dv2)
            {
                return false;
            }

            // CNPJ Válido!
            return true;
        }
        
        public static bool ValidarPlaca(string placa)
        {
            return !string.IsNullOrWhiteSpace(placa)
                   && RegexPlaca.IsMatch(placa);
        }
        
        private static int CalcularDigitoVerificadorMod11(int[] digitos, params int[] pesos)
        {
            var soma = 0;
            for (int i = 0; i < Math.Min(digitos.Length, pesos.Length); i++)
            {
                soma += digitos[i] * pesos[i];
            }

            var dv = 11 - (soma % 11);
            if (dv > 9)
            {
                dv = 0;
            }

            return dv;
        }
        
        private static bool ValidarNumerosIguais(int[] digitos)
        {
            if (digitos == null)
            {
                throw new ArgumentNullException("digitos");
            }

            if (digitos.Length == 1)
            {
                return true;
            }

            var referencia = digitos[0];

            for (int i = 1; i < digitos.Length; i++)
            {
                if (digitos[i] != referencia)
                {
                    return false;
                }
            }

            return true;
        }
        
        private static int[] ObterDigitos(string valor)
        {
            // verifica se contém somente dígitos
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            // cria o array para os dígitos
            var digitos = new int[valor.Length];

            // para cada caractere no valor
            for (int i = 0; i < valor.Length; i++)
            {
                var c = valor[i];

                // verifica se ele é digito
                if (!char.IsDigit(c))
                {
                    return null;
                }

                // converte para inteiro
                digitos[i] = (int)char.GetNumericValue(c);
            }

            return digitos;
        }
        
        public static bool ValidarIMEI(string imei)
        {
            return !string.IsNullOrWhiteSpace(imei)
                   && RegexIMEI.IsMatch(imei);
        }
        
        public static bool ValidarNumeroValido(object value)
        {
            if (value == null)
            {
                return false;
            }

            return !(AreEqual(value, Constants.BYTEINVALIDO)
                     || AreEqual(value, Constants.SHORTINVALIDO)
                     || AreEqual(value, Constants.LONGINVALIDO)
                     || AreEqual(value, Constants.INTINVALIDO));
        }
        
        private static bool AreEqual<T>(object value, T invalidValue)
        {
            if (value.GetType() != typeof(T))
            {
                return false;
            }

            return object.Equals(value, invalidValue);
        }
    }
    
    public static class Constants
    {
        public static readonly DateTime DATANULA = new DateTime(1, 1, 1);

        public static readonly DateTime DATAINVALIDA = new DateTime(1, 1, 2);

        public static readonly TimeSpan HORANULA = new TimeSpan(24, 0, 1);

        public static readonly TimeSpan HORAINVALIDA = new TimeSpan(24, 0, 2);

        public static readonly decimal DECIMALNULO = decimal.MaxValue - 1;

        public static readonly decimal DECIMALINVALIDO = decimal.MaxValue;

        public static readonly long LONGNULO = long.MaxValue - 1;

        public static readonly long LONGINVALIDO = long.MaxValue;

        public static readonly int INTNULO = int.MaxValue - 1;

        public static readonly int INTINVALIDO = int.MaxValue;

        public static readonly short SHORTNULO = short.MaxValue - 1;

        public static readonly short SHORTINVALIDO = short.MaxValue;

        public static readonly byte BYTENULO = byte.MaxValue - 1;

        public static readonly byte BYTEINVALIDO = byte.MaxValue;

        public static readonly int HORAS144 = 144;

        public static readonly int HORAS164 = 164;
    }
}