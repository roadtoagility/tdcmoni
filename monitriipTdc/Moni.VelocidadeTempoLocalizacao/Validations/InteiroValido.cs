namespace Moni.VelocidadeTempoLocalizacao.Validations
{
    public class InteiroValido
    {
        public static bool IsValid(object value)
        {
            return ValidationHelper.ValidarNumeroValido(value);
        }
    }
}