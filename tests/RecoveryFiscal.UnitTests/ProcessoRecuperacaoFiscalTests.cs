using RecoveryFiscal.Domain.Entities;
using RecoveryFiscal.Domain.Enums;
using Xunit;

namespace RecoveryFiscal.UnitTests;

public class ProcessoRecuperacaoFiscalTests
{
    [Fact]
    public void Deve_gerar_excecao_quando_data_analise_for_invalida()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new ProcessoRecuperacaoFiscal(
                "PROC-001",
                "123456789",
                "Cliente Teste",
                TipoCredito.CreditoPessoal,
                1000m,
                500m,
                new DateOnly(2026, 03, 10),
                new DateOnly(2026, 03, 09),
                StatusProcesso.EmAnalise,
                PrioridadeProcesso.Media,
                null,
                "tester"));

        Assert.Contains("data de análise", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
