using System;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class CNPJ
    {
        public static bool IsValid(string cnpj)
        {
            try
            {
                return string.IsNullOrEmpty(cnpj) || ValidationHelper.ValidarCnpj(cnpj);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}