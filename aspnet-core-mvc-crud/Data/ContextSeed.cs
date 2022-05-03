using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_mvc_crud.Models;

namespace aspnet_core_mvc_crud.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            //await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Tech.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Customer.ToString()));

        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "fercalmet@gmail.com",
                FirstName = "Fernando",
                LastName = "Calmet",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "@Pa$$word.123");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Customer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Tech.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
        public static void SeedTasks(RequestDbContext request)
        {
            ServiceRequest req = new ServiceRequest();
            req.Name = "first";
            req.Description = "description first";
            req.Picture = null;
            req.DateOfVisit = DateTime.Now;
            req.Status = Enums.Status.Awaiting;
            req.Address = "adres";
            req.AssignedTechnicianId = "149c4105-42ea-4195-abc2-f78ba038c983";
            req.CreatorId = "4a69cdcf-a36d-47a3-9aea-e1bb63416490";

            request.AddAsync(req);
            request.SaveChangesAsync();
        }
    }
}
