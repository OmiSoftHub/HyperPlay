﻿using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Models.Mappers;
using HyperPlayServer.Utilities;

namespace HyperPlayServer.Services;

public class UserService
{
    private UnitOfWork _unitOfWork;
    private UserMapper _userMapper;

    public UserService(UnitOfWork unitOfWork, UserMapper userMapper)
    {
        _unitOfWork = unitOfWork;
        _userMapper = userMapper;
    }

    public async Task<UserDto> GetUserDtoById(int userId)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        UserDto userDto = _userMapper.UsertoDto(user);

        return userDto;
    }

    public async Task UpdateUserBD(int userId, UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto), "El objeto UserDto no puede ser nulo.");
        }

        User existingUser = await _unitOfWork.UserRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException("Usuario no encontrado.");

        UpdateUserProperties(existingUser, userDto);

        await _unitOfWork.SaveAsync();
    }

    public async Task UpdatePassword(string password, int userId)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("La contraseña no puede ser nula o vacía.");
        }

        User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("No existe usuario con ese ID.");
        }

        string newHashPassword = PasswordHelper.Hash(password);

        if (user.HashPassword != newHashPassword)
        {
            user.HashPassword = newHashPassword;
        }

        await _unitOfWork.SaveAsync();
    }



    private static void UpdateUserProperties(User existingUser, UserDto userDto)
    {
        if (!string.IsNullOrEmpty(userDto.Name) && userDto.Name != existingUser.Name)
        {
            existingUser.Name = userDto.Name;
        }

        if (!string.IsNullOrEmpty(userDto.Surname) && userDto.Surname != existingUser.Surname)
        {
            existingUser.Surname = userDto.Surname;
        }

        if (!string.IsNullOrEmpty(userDto.Email) && userDto.Email != existingUser.Email)
        {
            existingUser.Email = userDto.Email;
        }

        if (!string.IsNullOrEmpty(userDto.Address) && userDto.Address != existingUser.Address)
        {
            existingUser.Address = userDto.Address;
        }
    }
}
