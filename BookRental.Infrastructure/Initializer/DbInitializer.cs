using System;
using System.Linq;
using BookRental.App.Config;
using BookRental.Domain.Models;
using BookRental.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BookRentalDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, BookRentalDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (_context.Roles.Any(r => r.Name == StaticDependencies.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(StaticDependencies.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDependencies.Role_User)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Rental Admin"
            }, "Admin123!").GetAwaiter().GetResult();

            var user = _context.AppUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");

            _userManager.AddToRoleAsync(user, StaticDependencies.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
