using AngelusTBlog.Data;
using AngelusTBlog.Enums;
using AngelusTBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AngelusTBlog.Services
{
    public class DataService
    {
        //  Create a new Identity type for the DB managent
        private readonly ApplicationDbContext _dbContext;

        //  Create a new Identity type for the Role managent
        private readonly RoleManager<IdentityRole> _roleManager;

        //  Create a new Identity type for the User
        private readonly UserManager<BlogUser> _userManager;

        //  Constructor Injection ApplicationDbContext
        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //  Public Data management class
        public async Task ManageDataAsync()
        {
            // Auto Create the DB fom the migrations calling MigrateAsync from Programs cs
            await _dbContext.Database.MigrateAsync();

            // Seed Roles
            await SeedRolesAsync();

            // Seed Users
            await SeedUsersAsync();
        }

        // Seed Roles Method
        private async Task SeedRolesAsync()
        {
            // If any roles exsist in rolses table don't do anything, else create roles.
            if (_dbContext.Roles.Any())
            {
                return;
            } 
            else 
            {
                // Create Roles.
                foreach(var role in Enum.GetNames(typeof(BlogRoles)))
                {
                    // Use the role manager to create role
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // Seed Users Method
        private async Task SeedUsersAsync()
        {
            // If any Users exsist in system don't do anything, else create Users.
            if (_dbContext.Users.Any())
            {
                return;
            } 
            else 
            {
                // Creates a new instance of Blog User
                var adminUser = new BlogUser()
                {
                    Email = "andysainttaylor@gmail.com",
                    UserName = "andysainttaylor@gmail.com",
                    FirstName = "Angelus",
                    LastName = "Taylor",
                    PhoneNumber = "+1 (778) 200 6424",
                    EmailConfirmed = true,
                     
                };

                // User user Manager to creat a new user that is an Admin
                await _userManager.CreateAsync(adminUser, "Pain@1242");

                // Add new user to the Aministrator Role
                await _userManager.AddToRoleAsync(adminUser, BlogRoles.Administrator.ToString());


                // Creates a new Moderator and in a new instance of Blog User. 
                var ModUser = new BlogUser()
                {
                    Email = "andysainttaylor@gmail.com",
                    UserName = "andysainttaylor@gmail.com",
                    FirstName = "Angelus",
                    LastName = "Taylor",
                    PhoneNumber = "+1 (778) 200 6424",
                    EmailConfirmed = true
                };

                // Complete the creation of Moderator and assign a password. 
                await _userManager.CreateAsync(ModUser, "Pain@1242");

                // Add new Moderators to the Moderators Role
                await _userManager.AddToRoleAsync(ModUser, BlogRoles.Moderators.ToString());
            }
        }

    }
}
