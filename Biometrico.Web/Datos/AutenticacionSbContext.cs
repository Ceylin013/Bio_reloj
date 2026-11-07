using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biometrico.Web.Datos;

public class AutenticacionDbContext : IdentityDbContext<IdentityUser>
{
    public AutenticacionDbContext(

        DbContextOptions<AutenticacionDbContext> options)

        : base(options)
    {
    }
}