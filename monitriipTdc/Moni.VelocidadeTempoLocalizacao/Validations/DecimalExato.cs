using System;

namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class DecimalExato
    {
        public static bool IsValid(decimal valor, int precisao, int escala )
        {
            try
            {
                string digitosEsquerda = Math.Truncate(valor).ToString();

                if (digitosEsquerda.Length > (precisao - escala))
                {
                    return false;
                }

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}