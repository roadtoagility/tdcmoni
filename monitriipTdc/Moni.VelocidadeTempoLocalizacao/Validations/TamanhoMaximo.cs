using System;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class TamanhoMaximo
    {
        public bool IsValid(object value)
        {
            try
            {
                var cnpj = (string)value;
                
                return string.IsNullOrEmpty(cnpj) || ValidationHelper.ValidarCnpj(cnpj);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}