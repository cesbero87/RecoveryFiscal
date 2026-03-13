namespace RecoveryFiscal.Application.Common.Abstractions;

public interface ICurrentUserService
{
    string GetCurrentUsername();
    string GetCorrelationId();
}
