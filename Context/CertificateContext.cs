using Microsoft.EntityFrameworkCore;
namespace CertificateAPI.Context;

public class CertificateContext: DbContext
{
    public CertificateContext(DbContextOptions<CertificateContext> options): base(options){}
    public DbSet<Certificate> Certificates { get; set; }
}