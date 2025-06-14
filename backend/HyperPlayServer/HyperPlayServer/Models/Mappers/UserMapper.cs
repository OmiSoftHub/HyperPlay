using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Dtos;

namespace HyperPlayServer.Models.Mappers;

public class UserMapper
{
    public UserDto UsertoDto(User user)
    {
        return new UserDto
        {
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address
        };
    }
}