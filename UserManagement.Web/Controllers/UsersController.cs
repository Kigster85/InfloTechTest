using System.Linq;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    // New action method to filter users by their active state (true)
    [HttpGet("active")]
    public IActionResult ActiveUsers()
    {
        var items = _userService.FilterByActive(true).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth

        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View("List", model); // Reuse the List view to display filtered users
    }

    // New action method to filter users by their active state (false)
    [HttpGet("inactive")]
    public IActionResult InactiveUsers()
    {
        var items = _userService.FilterByActive(false).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth

        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View("List", model); // Reuse the List view to display filtered users
    }

    // New action method to filter users by their active state (false)
    [HttpGet("view")]
    public IActionResult ViewUsers()
    {
        var items = _userService.GetById().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth

        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View("List", model); // Reuse the List view to display filtered users
    }
}
