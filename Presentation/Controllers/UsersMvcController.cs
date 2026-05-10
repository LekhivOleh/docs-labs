using docs_project.Application.Interfaces.Services;
using docs_project.Domain.Entities;
using docs_project.Presentation.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers;

[Authorize]
[Route("Users")]
public class UsersMvcController : Controller
{
    private readonly IUserService _userService;

    public UsersMvcController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new UserCreateViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = model.Username,
            Email = model.Email,
            Password = model.Password
        };

        await _userService.CreateAsync(user);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null) return NotFound();
        return View(new UserEditViewModel { Id = user.Id, Username = user.Username, Email = user.Email });
    }

    [HttpPost("{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _userService.GetByIdAsync(id);
        if (user is null) return NotFound();
        user.Username = model.Username;
        user.Email = model.Email;
        await _userService.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("import")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            TempData["Error"] = "Please select a CSV file.";
            return RedirectToAction(nameof(Index));
        }

        await using var stream = file.OpenReadStream();
        var imported = await _userService.ImportFromCsvAsync(stream);
        TempData["Info"] = $"Imported {imported} users.";

        return RedirectToAction(nameof(Index));
    }
}

