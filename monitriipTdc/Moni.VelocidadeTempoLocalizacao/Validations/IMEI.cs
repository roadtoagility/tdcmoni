using System;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class IMEI
    {
        public bool IsValid(object value)
        {
            try
            {
                if (value == null)
                {
                    return true;
                }

                var imei = (string)value;

                return ValidationHelper.ValidarIMEI(imei);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}