using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            try 
            {
                var HasRoles = roleManager.Roles.Any();
                var HasUsers = userManager.Users.Any();
                if(HasRoles && HasUsers)
                {
                    return false;
                }
                if(!HasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                        new (){Name = "SuperAdmin" },
                        new (){Name = "Admin" },
                    };
                    foreach (var role in roles)
                    {
                        var roleExists = roleManager.RoleExistsAsync(role.Name!).Result;
                        if (!roleExists)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser
                    {
                            FirstName = "Raghad",
                            LastName ="Nour",
                            Email ="RaghadNour@gmail.com",
                            PhoneNumber ="01111111111",
                            UserName ="RaghadNour"

                    };
                    userManager.CreateAsync(MainAdmin,"P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin,"SuperAdmin").Wait();

                    var Admin = new ApplicationUser
                    {
                        FirstName = "Salah",
                        LastName = "Nour",
                        Email = "SalahNour@gmail.com",
                        PhoneNumber = "01222222222",
                        UserName = "SalahNour"

                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();

                }
                return true;

            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while seeding the database: {ex.Message}", ex);
                
            }
        }
    }
}
