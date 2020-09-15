using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Data.Initializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DBInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (_db.Roles.Any(r => r.Name == SD.Admin))
            {
                return;
            }
            _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Manager)).GetAwaiter().GetResult();
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "A Vazul"
            }, "Admin123*").GetAwaiter().GetResult();
            IdentityUser user = _db.Users.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();

        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (await _db.Roles.AnyAsync(r => r.Name == SD.Admin))
            {
                return;
            }
            var admin = _roleManager.CreateAsync(new IdentityRole(SD.Admin));
            var manager = _roleManager.CreateAsync(new IdentityRole(SD.Manager));
            var adminUser = _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "A Vazul"
            }, "Admin123*");
            var results = await Task.WhenAll(admin, manager, adminUser);
            IdentityUser user = await _db.Users.Where(u => u.Email == "admin@gmail.com").FirstOrDefaultAsync();
            await _userManager.AddToRoleAsync(user, SD.Admin);

        }
    }
}
