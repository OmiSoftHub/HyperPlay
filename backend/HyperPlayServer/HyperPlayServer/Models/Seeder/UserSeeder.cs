using HyperPlayServer.Models.Database;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Utilities;

namespace HyperPlayServer.Models.Seeder;

public class UserSeeder
{
    private readonly MyDbContext _context;

    public UserSeeder(MyDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Users.Any())
        {
            Console.WriteLine("Usuarios ya existen en la base de datos, no se insertarán duplicados.");
            return;
        }

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Hyper",
                Surname = "Games",
                Email = "hyperplay24@gmail.com",
                HashPassword = PasswordHelper.Hash("hyperplay"),
                Rol = "Admin",
                Address = "Calle 123"
            },
            new User
            {
                Id = 2,
                Name = "User",
                Surname = "User",
                Email = "user@user.com",
                HashPassword = PasswordHelper.Hash("user"),
                Rol = "User",
                Address = "Calle 456"
            }
        };

        var carts = new List<Cart>
        {
            new Cart
            {
                Id = 1,
                UserId = 1
            },

            new Cart
            {
                Id = 2,
                UserId = 2
            }
        };

        _context.Carts.AddRange(carts);
        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
 }
