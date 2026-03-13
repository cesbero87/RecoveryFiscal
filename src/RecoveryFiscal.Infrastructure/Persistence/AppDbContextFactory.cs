using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RecoveryFiscal.Infrastructure.Persistence;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            "Server=localhost;Port=3307;Database=recovery_fiscal;User=recovery_user;Password=recovery_pwd_123!;AllowPublicKeyRetrieval=True;SslMode=None";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AppDbContext(optionsBuilder.Options);
    }
}