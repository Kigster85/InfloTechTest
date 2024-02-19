using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        // Use LINQ to filter users by their IsActive property
        return _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public IEnumerable<User> GetById() => _dataAccess.GetAll<User>();
    // Method to create a new user
    public void CreateUser(User user)
    {
        // Ensure IsActive is set accordingly
        user.IsActive = true; // or false depending on your logic
        _dataAccess.Create(user);
    }

    // Method to update an existing user
    public void UpdateUser(User user)
    {
        // Update IsActive and other properties as needed
        _dataAccess.Update(user);
    }
    public void ViewUser(User user)
    {
        // Update IsActive and other properties as needed
        _dataAccess.Equals(user);
    }
}
