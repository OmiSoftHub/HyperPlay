﻿using System.ComponentModel;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Models.Mappers;

namespace HyperPlayServer.Services;

public class AdminUserService
{
    UnitOfWork _unitOfWork { get; set; }
    AdminMapper _adminMapper { get; set; }
    private SmartSearchService smartSearchService;

    public AdminUserService(AdminMapper adminMapper, UnitOfWork unitOfWork)
    {
        _adminMapper = adminMapper;
        _unitOfWork = unitOfWork;
        smartSearchService = new SmartSearchService();
    }

    public async Task<List<AdminUserDto>> GetListUser()
    {
        List<User> users = await _unitOfWork.UserRepository.GetAllUserAsync();
        return _adminMapper.ToListAdminUser(users);
    }

    public async Task<List<AdminUserDto>> GetGameBySearch(string search)
    {
        List<User> users = new List<User>();

        if (string.IsNullOrWhiteSpace(search))
        {
            throw new ArgumentException("La búsqueda no puede estar vacía.");
        }

        List<string> names = await _unitOfWork.UserRepository.GetAllName();

        IEnumerable<string> matchedTitles = smartSearchService.Search(search, names);

        if (matchedTitles != null && matchedTitles.Any())
        {
            users = await _unitOfWork.UserRepository.GetGamesByNames(matchedTitles);
        }

        List<AdminUserDto> adminUsers = _adminMapper.ToListAdminUser(users);

        return adminUsers;

    }

    public async Task PutRolUserById(int userId)
    {
        const string ADMIN = "Admin";
        const string USER = "User";

        User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"El usuario con ID {userId} no fue encontrado.");
        }

        if (user.Rol == ADMIN) {

            user.Rol = USER;

        } else
        {
            user.Rol = ADMIN;
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteUserById(int userId)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"El usuario con ID {userId} no fue encontrado.");
        }

        _unitOfWork.UserRepository.Delete(user);
        await _unitOfWork.SaveAsync();
    }

    
}
