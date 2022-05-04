using System.Globalization;
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
    
    bool Match(string source, string value, string matchType)
    {
        switch (matchType.ToLower())
        {
            case "icontains":
                return source.ToLower().Contains(value.ToLower());
            case "contains":
                return source.Contains(value);
            case "iequals":
                return string.Equals(source, value, StringComparison.CurrentCultureIgnoreCase);
            case "equals":
                return string.Equals(source, value);
            default:
                return true;
        }
    }

    private IQueryable<Certificate> Filter(
        IQueryable<Certificate> certificates,
        string filterBy,
        string filterValue,
        string matchType = "iContains"
    )
    {
        return filterBy.ToLower() switch
        {
            "passport" => certificates.Where(
                s => Match(s.Passport ?? string.Empty, filterValue, matchType)),
            "username" => certificates.Where(
                s => Match(s.UserName ?? string.Empty, filterValue, matchType)),
            "birthdate" => certificates.Where(
                s => Match(s.BirthDate.ToString(CultureInfo.CurrentCulture), filterValue, matchType)),
            "startdate" => certificates.Where(
                s => Match(s.StartDate.ToString(CultureInfo.CurrentCulture), filterValue, matchType)),
            "enddate" => certificates.Where(
                s => Match(s.EndDate.ToString(CultureInfo.CurrentCulture), filterValue, matchType)),
            "vaccine" => certificates.Where(
                s => Match(s.Vaccine ?? string.Empty, filterValue, matchType)),
            "id" => certificates.Where(
                s => Match(s.Id.ToString(), filterValue, matchType)),
            "any" => certificates.Where(
                s => Match(s.ToString(), filterValue, matchType)),
            _ => certificates
        };
    }
    
    private IQueryable<Certificate> Sort(
        IQueryable<Certificate> certificates,
        string field,
        bool orderAsc = true
    )
    {
        IOrderedQueryable<Certificate> sortedCertificates = field.ToLower() switch
        {
            "Id" => certificates.OrderBy(s => s.Id),
            "Passport" => certificates.OrderBy(s => s.Passport),
            "UserName" => certificates.OrderBy(s => s.UserName),
            "BirthDate" => certificates.OrderBy(s => s.BirthDate),
            "StartDate" => certificates.OrderBy(s => s.StartDate),
            "EndDate" => certificates.OrderBy(s => s.EndDate),
            "Vaccine" => certificates.OrderBy(s => s.Vaccine),
            _ => (IOrderedQueryable<Certificate>) certificates
        };
        return orderAsc ? sortedCertificates : sortedCertificates.Reverse();
    }

    [HttpGet]
    public async Task<ActionResult<Certificate>> Get(string sortOrder, string filterBy, string filterValue)
    {
        IQueryCollection query = Request.Query;
        
        IQueryable<Certificate> dbCertificates = _context.Certificates.AsQueryable();

        return Ok(dbCertificates);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Certificate>> Retrieve(int id)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return BadRequest("Certificate not found");
        return Ok(dbCertificate);
    }

    [HttpPost]
    public async Task<ActionResult<Certificate>> Post(Certificate certificate)
    {
        _context.Certificates.Add(certificate);
        await _context.SaveChangesAsync();

        return Ok(await _context.Certificates.ToListAsync());
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Certificate>> Put(int id)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return BadRequest("Certificate not found");

        dbCertificate.Passport = dbCertificate.Passport;
        dbCertificate.UserName = dbCertificate.UserName;
        dbCertificate.BirthDate = dbCertificate.BirthDate;
        dbCertificate.StartDate = dbCertificate.StartDate;
        dbCertificate.EndDate = dbCertificate.EndDate;
        dbCertificate.Vaccine = dbCertificate.Vaccine;
        
        await _context.SaveChangesAsync();

        return Ok(dbCertificate);
    }
    
    [HttpPatch]
    public async Task<ActionResult<Certificate>> Patch(Certificate certificate)
    {
        _context.Certificates.Add(certificate);
        await _context.SaveChangesAsync();

        return Ok(await _context.Certificates.ToListAsync());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Certificate>> Delete(int id)
    {
        Certificate? dbCertificate = await _context.Certificates.FindAsync(id);
        if (dbCertificate == null)
            return BadRequest("Certificate not found");

        _context.Certificates.Remove(dbCertificate);
        await _context.SaveChangesAsync();

        return Ok();
    }
}