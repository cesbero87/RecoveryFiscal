namespace RecoveryFiscal.Application.Common.Exceptions;

public class ValidationAppException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
        : base("Ocorreram erros de validação.")
    {
        Errors = errors;
    }
}
