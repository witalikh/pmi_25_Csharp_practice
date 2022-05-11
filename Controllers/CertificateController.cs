using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CertificateAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificateController : ControllerBase
{
    private CertificateContext _context;

    public CertificateController(CertificateContext context)
    {
        _context = context;
    }

    private IQueryable<Certificate> Filter(
        IQueryable<Certificate> certificates,
        string? filterBy,
        string? filterValue
    )
    {
        return (filterBy ?? string.Empty).ToLower() switch
        {
            "passport" => certificates.Where(
                s => EF.Functions.ILike(s.Passport, $"%{filterValue}%")),
            "username" => certificates.Where(
                s => EF.Functions.ILike(s.UserName, $"%{filterValue}%")),
            "birthdate" => certificates.Where(
                s => EF.Functions.ILike(s.BirthDate.ToString(), $"%{filterValue}%")),
            "startdate" => certificates.Where(
                s => EF.Functions.ILike(s.StartDate.ToString(), $"%{filterValue}%")),
            "enddate" => certificates.Where(
                s => EF.Functions.ILike(s.EndDate.ToString(), $"%{filterValue}%")),
            "vaccine" => certificates.Where(
                s => EF.Functions.ILike(s.Vaccine, $"%{filterValue}%")),
            "id" => certificates.Where(
                s => EF.Functions.ILike(s.Id.ToString(), $"%{filterValue}%")),
            "any" => certificates.Where(
                s => EF.Functions.ILike(
                    s.StartDate.ToString() + '$' +
                    s.EndDate + '$' +
                    s.BirthDate + '$' +
                    s.UserName + '$' +
                    s.Id + '$' +
                    s.Passport + '$' +
                    s.Vaccine,
                    $"%{filterValue}%")),
            _ => certificates
        };
    }
    
    private IQueryable<Certificate> Sort(
        IQueryable<Certificate> certificates,
        string? field
    )
    {
        if (field != null)
        {
            bool orderAsc = field.Contains("_desc");
            field = field?.Replace("_desc", "");
            IOrderedQueryable<Certificate> sortedCertificates = field.ToLower() switch
            {
                "id" => certificates.OrderBy(s => s.Id),
                "passport" => certificates.OrderBy(s => s.Passport),
                "username" => certificates.OrderBy(s => s.UserName),
                "birthdate" => certificates.OrderBy(s => s.BirthDate),
                "startdate" => certificates.OrderBy(s => s.StartDate),
                "enddate" => certificates.OrderBy(s => s.EndDate),
                "vaccine" => certificates.OrderBy(s => s.Vaccine),
                _ => certificates.OrderBy(s => s.Id)
            };
            return orderAsc ? sortedCertificates.Reverse() : sortedCertificates;
        }

        return certificates;
    }

    [HttpGet]
    public async Task<ActionResult<Certificate>> List(
        string? order = null, 
        string? filterBy = null, 
        string? filterValue = null
    )
    {
        var dbCertificates = _context.Certificates.AsQueryable();
        dbCertificates = Filter(dbCertificates, filterBy, filterValue);
        dbCertificates = Sort(dbCertificates, order);
        return Ok(dbCertificates);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Certificate>> Retrieve(int id)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return NotFound(new Dictionary<string, string>{{"detail", "Certificate not found"}});
        return Ok(dbCertificate);
    }

    [HttpPost]
    public async Task<ActionResult<Certificate>> Create(Certificate certificate)
    {
        //_context.Certificates
        _context.Certificates.Add(certificate);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Certificate>> Update(int id, Certificate certificate)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return NotFound(new Dictionary<string, string>{{"detail", "Certificate not found"}});

        dbCertificate.Passport = certificate.Passport;
        dbCertificate.UserName = certificate.UserName;
        dbCertificate.BirthDate = certificate.BirthDate;
        dbCertificate.EndDate = certificate.EndDate;
        dbCertificate.StartDate = certificate.StartDate;
        dbCertificate.Vaccine = certificate.Vaccine;

        await _context.SaveChangesAsync();
        return Ok(certificate);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Certificate>> Delete(int id)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return NotFound(new Dictionary<string, string>{{"detail", "Certificate not found"}});

        _context.Certificates.Remove(dbCertificate);
        await _context.SaveChangesAsync();

        return Ok();
    }
}