using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace CertificateAPI.Context;

public class CertificateContext : IdentityDbContext<User>
{
    public CertificateContext(DbContextOptions<CertificateContext> options) : base(options)
    {
    }

    public DbSet<Certificate> Certificates { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)  
    {  
        base.OnModelCreating(builder);  
    }
}