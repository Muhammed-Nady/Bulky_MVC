using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DbInitializer {
    public class DbInitializer : IDbInitializer {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db) {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }


        public async Task InitializeAsync() {


            //migrations if they are not applied
            try {
                if ((await _db.Database.GetPendingMigrationsAsync()).Any()) {
                    await _db.Database.MigrateAsync();
                }
            }
            catch(Exception ex) { }



            //create roles if they are not created
            if (!await _roleManager.RoleExistsAsync(SD.Role_Customer)) {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
            }

            //create admin user if it does not exist
            if (await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == "admin@bulkybook.com") == null) {
                await _userManager.CreateAsync(new ApplicationUser {
                    UserName = "admin@bulkybook.com",
                    Email = "admin@bulkybook.com",
                    Name = "Admin",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 Ave",
                    State = "IL",
                    PostalCode = "23422",
                    City = "Chicago"
                }, "Admin123*");

                ApplicationUser user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == "admin@bulkybook.com");
                await _userManager.AddToRoleAsync(user, SD.Role_Admin);
            }

            if (!await _db.ProductImages.AnyAsync()) {
                _db.ProductImages.AddRange(
                    new ProductImage { ProductId = 1, ImageUrl = @"\images\products\product-1\0f09982d-d0b7-4571-9ada-1267ad925470.jpg" },
                    new ProductImage { ProductId = 1, ImageUrl = @"\images\products\product-1\3cc41d0b-70cc-4203-a5e2-ff90aa05fc79.jpg" },
                    new ProductImage { ProductId = 2, ImageUrl = @"\images\products\product-2\35a155bd-4d84-4926-936b-bc1c0cff2017.jpg" },
                    new ProductImage { ProductId = 2, ImageUrl = @"\images\products\product-2\6c2133da-211f-4ef1-90a9-dd1cf3b66f38.jpg" },
                    new ProductImage { ProductId = 3, ImageUrl = @"\images\products\product-3\1f36fa1f-7f9a-4b4f-84d2-eafbf69a6220.jpg" },
                    new ProductImage { ProductId = 3, ImageUrl = @"\images\products\product-3\748b27d9-9802-42f6-adab-8df55aaf4342.jpg" },
                    new ProductImage { ProductId = 4, ImageUrl = @"\images\products\product-4\6b702e26-aa2b-47b1-bab3-f885486939e3.jpg" },
                    new ProductImage { ProductId = 4, ImageUrl = @"\images\products\product-4\8be61614-113a-4dd5-a2ff-d1eb70e16f0b.jpg" },
                    new ProductImage { ProductId = 5, ImageUrl = @"\images\products\product-5\1cbca164-761a-4c55-9459-82d6f2c9e0c1.jpg" },
                    new ProductImage { ProductId = 5, ImageUrl = @"\images\products\product-5\7f0d6e2a-1a34-4c44-9596-0893338718bf.jpg" },
                    new ProductImage { ProductId = 6, ImageUrl = @"\images\products\product-6\3772c79c-f3dd-4f33-ba5a-e4bb6e198375.jpg" },
                    new ProductImage { ProductId = 6, ImageUrl = @"\images\products\product-6\52814dff-4844-44a5-8e73-95b0e9c22c50.jpg" }
                );
                await _db.SaveChangesAsync();
            }
        }
    }
}
