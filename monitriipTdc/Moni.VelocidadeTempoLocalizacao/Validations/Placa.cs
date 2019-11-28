using System;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class Placa
    {
        public static bool IsValid(object value)
        {
            try
            {
                var placa = (string)value;

                return string.IsNullOrEmpty(placa) || ValidationHelper.ValidarPlaca(placa);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}